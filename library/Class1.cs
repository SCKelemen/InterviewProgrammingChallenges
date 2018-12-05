using System;
using System.Collections;
using System.Collections.Generic;

namespace library
{
    public class Class1
    {
    }

    public abstract class Event 
    {
        public DateTime EventDate { get; }
    }

    public enum EventType 
    {
        EntryEvent,
        DepartureEvent
    }
    public class TravelEvent : Event
    {
        private readonly EventType _eventType;
        public TravelEvent(EventType eventType) 
        {
            _eventType = eventType;
        }

        public EventType EventType => _eventType;
    }

    public class EventStream<T> : List<T> where T : Event
    {

    }

    public class ParseHazard 
    {
        private string _message;
        public ParseHazard(string message) 
        {
            _message =  message;
        }

        public string Message => _message;

    }
    public class EventParser<T> where T : Event  
    {
        // state
        int _index = 0;
        T currentEvent; // current item under inspection
        EventStream<T> _stream; 
        int _runningSum = 0;
        public EventParser(EventStream<T> stream) 
        {
            _stream = stream;
        }

        public T MoveNext() 
        {
            return HasNext() ? _stream[_index++] : null;
        }

        public bool HasNext() 
        {
            return _index < _stream.Count;
        }

        public T Peak() 
        {
            return HasNext() ? _stream[_index + 1] : null;
        }
    }

    public class TravelEventParser //: EventParser
    {
        // state
        List<ParseHazard> _hazards;
        TravelEvent _currentEvent; // event under inspection
        EventParser<TravelEvent> _parser;
        DateTime _top;
        int _runningSum = 0;
        bool _parsed = false;
        public TravelEventParser(EventStream<TravelEvent> stream, DateTime year) // : base(stream)
        {
           // _currentEvent = base.HasNext() ? base.Peak() : null;
            _hazards = new List<ParseHazard>();
            _parser = new EventParser<TravelEvent>(stream);
            _currentEvent = _parser.HasNext() ? _parser.MoveNext() : null;
            if (_currentEvent != null) 
            {
                _top = _currentEvent.EventType == EventType.DepartureEvent ? _currentEvent.EventDate : new DateTime(year.Year, 12, 31);
            } else {
                // if _currentEvent is null, the eventstream is empty, and it's fully parsed. Return 0;
                _parsed = true;
            }
        }

        public int Parse() 
        {
            if (_parsed) return _runningSum;
            
            while(_parser.HasNext()) 
            {
                TravelEvent next = _parser.Peak();
                if (_currentEvent.EventType == next.EventType) 
                {
                    _hazards.Add(new ParseHazard("Event mismatch detected."));
                    _currentEvent = _parser.MoveNext();
                    continue;
                }

                if (next.EventType == EventType.EntryEvent) 
                {


                }

            }
            return 0;
         }

    }
}
