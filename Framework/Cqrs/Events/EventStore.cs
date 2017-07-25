﻿#region Copyright
// // -----------------------------------------------------------------------
// // <copyright company="Chinchilla Software Limited">
// // 	Copyright Chinchilla Software Limited. All rights reserved.
// // </copyright>
// // -----------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using cdmdotnet.Logging;
using Cqrs.Domain;
using Cqrs.Messages;

namespace Cqrs.Events
{
	/// <summary>
	/// Stores instances of <see cref="IEvent{TAuthenticationToken}"/> for replay, <see cref="IAggregateRoot{TAuthenticationToken}"/> and <see cref="ISaga{TAuthenticationToken}"/> rehydration.
	/// </summary>
	/// <typeparam name="TAuthenticationToken">The <see cref="Type"/> of the authentication token.</typeparam>
	public abstract class EventStore<TAuthenticationToken> : IEventStore<TAuthenticationToken>
	{
		protected const string CqrsEventStoreStreamNamePattern = "{0}/{1}";

		/// <summary>
		/// The <see cref="IEventBuilder{TAuthenticationToken}"/> used to build events.
		/// </summary>
		protected IEventBuilder<TAuthenticationToken> EventBuilder { get; set; }

		/// <summary>
		/// The <see cref="IEventDeserialiser{TAuthenticationToken}"/> used to deserialise events.
		/// </summary>
		protected IEventDeserialiser<TAuthenticationToken> EventDeserialiser { get; set; }

		protected ITelemetryHelper TelemetryHelper { get; set; }

		protected ILogger Logger { get; private set; }

		/// <summary>
		/// Instantiates a new instance of <see cref="EventStore{TAuthenticationToken}"/>.
		/// </summary>
		protected EventStore(IEventBuilder<TAuthenticationToken> eventBuilder, IEventDeserialiser<TAuthenticationToken> eventDeserialiser, ILogger logger)
		{
			EventBuilder = eventBuilder;
			EventDeserialiser = eventDeserialiser;
			Logger = logger;
			TelemetryHelper = new NullTelemetryHelper();
		}

		/// <summary>
		/// Saves the provided <paramref name="event"/>.
		/// </summary>
		/// <typeparam name="T">The <see cref="Type"/> of the <see cref="IAggregateRoot{TAuthenticationToken}"/> the <see cref="IEvent{TAuthenticationToken}"/> was raised in.</typeparam>
		/// <param name="event">The <see cref="IEvent{TAuthenticationToken}"/> to be saved.</param>
		public virtual void Save<T>(IEvent<TAuthenticationToken> @event)
		{
			Save(typeof(T), @event);
		}

		protected virtual string GenerateStreamName(Type aggregateRootType, IEvent<TAuthenticationToken> @event)
		{
			return GenerateStreamName(aggregateRootType, @event.Id);
		}

		protected virtual string GenerateStreamName(Type aggregateRootType, Guid aggregateId)
		{
			return string.Format(CqrsEventStoreStreamNamePattern, aggregateRootType.FullName, aggregateId);
		}

		/// <summary>
		/// Saves the provided <paramref name="event"/>.
		/// </summary>
		/// <param name="aggregateRootType"> <see cref="Type"/> of the <see cref="IAggregateRoot{TAuthenticationToken}"/> the <see cref="IEvent{TAuthenticationToken}"/> was raised in.</param>
		/// <param name="event">The <see cref="IEvent{TAuthenticationToken}"/> to be saved.</param>
		public virtual void Save(Type aggregateRootType, IEvent<TAuthenticationToken> @event)
		{
			Logger.LogDebug(string.Format("Saving aggregate root event type '{0}'", @event.GetType().FullName), string.Format("{0}\\Save", GetType().Name));
			EventData eventData = EventBuilder.CreateFrameworkEvent(@event);
			string streamName = GenerateStreamName(aggregateRootType, @event);
			eventData.AggregateId = streamName;
			eventData.AggregateRsn = @event.Id;
			eventData.Version = @event.Version;
			eventData.CorrelationId = @event.CorrelationId;
			PersistEvent(eventData);
			Logger.LogInfo(string.Format("Saving aggregate root event type '{0}'... done", @event.GetType().FullName), string.Format("{0}\\Save", GetType().Name));
			TelemetryHelper.TrackMetric(string.Format("Cqrs/EventStore/Save/{0}", streamName), 1);
		}

		/// <summary>
		/// Gets a collection of <see cref="IEvent{TAuthenticationToken}"/> for the <typeparamref name="T">aggregate root</typeparamref> with the ID matching the provided <paramref name="aggregateId"/>.
		/// </summary>
		/// <typeparam name="T">The <see cref="Type"/> of the <see cref="IAggregateRoot{TAuthenticationToken}"/> the <see cref="IEvent{TAuthenticationToken}"/> was raised in.</typeparam>
		/// <param name="aggregateId">The <see cref="IAggregateRoot{TAuthenticationToken}.Id"/> of the <see cref="IAggregateRoot{TAuthenticationToken}"/>.</param>
		/// <param name="useLastEventOnly">Loads only the last event<see cref="IEvent{TAuthenticationToken}"/>.</param>
		/// <param name="fromVersion">Load events starting from this version</param>
		public virtual IEnumerable<IEvent<TAuthenticationToken>> Get<T>(Guid aggregateId, bool useLastEventOnly = false, int fromVersion = -1)
		{
			IEnumerable<IEvent<TAuthenticationToken>> results = Get(typeof (T), aggregateId, useLastEventOnly, fromVersion);
			TelemetryHelper.TrackMetric(string.Format("Cqrs/EventStore/Get/{0}", GenerateStreamName(typeof(T), aggregateId)), results.Count());

			return results;
		}

		/// <summary>
		/// Gets a collection of <see cref="IEvent{TAuthenticationToken}"/> for the <see cref="IAggregateRoot{TAuthenticationToken}"/> of type <paramref name="aggregateRootType"/> with the ID matching the provided <paramref name="aggregateId"/>.
		/// </summary>
		/// <param name="aggregateRootType"> <see cref="Type"/> of the <see cref="IAggregateRoot{TAuthenticationToken}"/> the <see cref="IEvent{TAuthenticationToken}"/> was raised in.</param>
		/// <param name="aggregateId">The <see cref="IAggregateRoot{TAuthenticationToken}.Id"/> of the <see cref="IAggregateRoot{TAuthenticationToken}"/>.</param>
		/// <param name="useLastEventOnly">Loads only the last event<see cref="IEvent{TAuthenticationToken}"/>.</param>
		/// <param name="fromVersion">Load events starting from this version</param>
		public abstract IEnumerable<IEvent<TAuthenticationToken>> Get(Type aggregateRootType, Guid aggregateId, bool useLastEventOnly = false, int fromVersion = -1);

		/// <summary>
		/// Get all <see cref="IEvent{TAuthenticationToken}"/> instances for the given <paramref name="correlationId"/>.
		/// </summary>
		/// <param name="correlationId">The <see cref="IMessage.CorrelationId"/> of the <see cref="IEvent{TAuthenticationToken}"/> instances to retrieve.</param>
		public abstract IEnumerable<EventData> Get(Guid correlationId);

		protected abstract void PersistEvent(EventData eventData);
	}
}