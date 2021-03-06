﻿using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace RPGCore.Events
{
	[JsonObject]
	public sealed class EventDictionary<TKey, TValue> : IEventDictionary<TKey, TValue>
	{
		private readonly Dictionary<TKey, TValue> collection;

		[JsonIgnore]
		public int Count => collection.Count;

		[JsonIgnore]
		public EventDictionaryHandlerCollection<TKey, TValue> Handlers { get; }

		public TValue this[TKey key] => collection[key];

		public EventDictionary()
		{
			Handlers = new EventDictionaryHandlerCollection<TKey, TValue>(this);
			collection = new Dictionary<TKey, TValue>();
		}

		public bool ContainsKey(TKey key)
		{
			return collection.ContainsKey(key);
		}

		public void Add(TKey key, TValue value)
		{
			collection.Add(key, value);

			Handlers.InvokeAdd(key, value);
		}

		public bool Remove(TKey key)
		{
			if (!collection.TryGetValue(key, out var eventObject))
			{
				return false;
			}

			bool result = collection.Remove(key);
			if (result)
			{
				Handlers.InvokeRemoved(key, eventObject);
			}

			return result;
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return collection.TryGetValue(key, out value);
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return collection.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return collection.GetEnumerator();
		}
	}
}
