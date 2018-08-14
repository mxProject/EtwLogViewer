using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mxProject.Tools.EtwLogViewer
{

    /// <summary>
    /// 
    /// </summary>
    internal enum TraceEventFieldIndex
    {

        /// <summary>
        /// 
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// EventTypeUserData is a field users get to use to attach their own data on a per-event-type basis.
        /// </summary>
        EventTypeUserData,

        /// <summary>
        /// The GUID that uniquely identifies the Provider for this event. This can return Guid.Empty for classic (Pre-VISTA) ETW providers.
        /// </summary>
        ProviderGuid,

        /// <summary>
        /// The name of the provider associated with the event. It may be of the form Provider(GUID) or UnknownProvider in some cases but is never null.
        /// </summary>
        ProviderName,

        /// <summary>
        /// A name for the event. This is simply the concatenation of the task and opcode names (separated by a /). If the event has no opcode, then the event name is just the task name.
        /// </summary>
        EventName,

        /// <summary>
        /// Returns the provider-specific integer value that uniquely identifies event within the scope of the provider. (Returns 0 for classic (Pre-VISTA) ETW providers).
        /// </summary>
        ID,

        /// <summary>
        /// Events for a given provider can be given a group identifier (integer) called a Task that indicates the broad area within the provider that the event pertains to (for example the Kernel provider has Tasks for Process, Threads, etc).
        /// </summary>
        Task,

        /// <summary>
        /// The human readable name for the event's task (group of related events) (eg. process, thread, image, GC, ...). May return a string Task(GUID) or Task(TASK_NUM) if no good symbolic name is available. It never returns null.
        /// </summary>
        TaskName,

        /// <summary>
        /// An opcode is a numeric identifier (integer) that identifies the particular event within the group of events identified by the event's task. Often events have opcode 'Info' (0), which is the default. This value is interpreted as having no-opcode (the task is sufficient to identify the event). Generally the most useful opcodes are the Start and Stop opcodes which are used to indicate the beginning and the end of a interval of time. Many tools will  match up start and stop opcodes automatically and compute durations.
        /// </summary>
        Opcode,

        /// <summary>
        /// Returns the human-readable string name for the Opcode property.
        /// </summary>
        OpcodeName,

        /// <summary>
        /// The verbosity of the event (Fatal, Error, ..., Info, Verbose)
        /// </summary>
        Level,

        /// <summary>
        /// The version number for this event. The only compatible change to an event is to add new properties at the end. When this is done the version numbers is incremented.
        /// </summary>
        Version,

        /// <summary>
        /// ETW Event providers can specify a 64 bit bitfield called 'keywords' that define provider-specific groups of events which can be enabled and disabled independently. Each event is given a keywords mask that identifies which groups the event belongs to. This property returns this mask.
        /// </summary>
        Keywords,

        /// <summary>
        /// A Channel is a identifier (integer) that defines an 'audience' for the event (admin, operational, ...). Channels are only used for Windows Event Log integration.
        /// </summary>
        Channel,

        /// <summary>
        /// The time of the event. You may find TimeStampRelativeMSec more convenient.
        /// </summary>
        TimeStamp,

        /// <summary>
        /// Returns a double representing the number of milliseconds since the beginning of the session.
        /// </summary>
        TimeStampRelativeMSec,

        /// <summary>
        /// The thread ID for the thread that logged the event. This field may return -1 for some events when the thread ID is not known.
        /// </summary>
        ThreadID,

        /// <summary>
        /// The process ID of the process which logged the event. This field may return -1 for some events when the process ID is not known.
        /// </summary>
        ProcessID,

        /// <summary>
        /// Returns a short name for the process. This the image file name (without the path or extension), or if that is not present, then the string 'Process(XXXX)'
        /// </summary>
        ProcessName,

        /// <summary>
        /// The processor Number (from 0 to TraceEventSource.NumberOfProcessors) that logged this event.
        /// </summary>
        ProcessorNumber,

        /// <summary>
        /// Get the size of a pointer associated with process that logged the event (thus it is 4 for a 32 bit process).
        /// </summary>
        PointerSize,

        /// <summary>
        /// Conceptually every ETW event can be given a ActivityID (GUID) that uniquely identifies the logical work being carried out (the activity). This property returns this GUID. Can return Guid.Empty if the thread logging the event has no activity ID associated with it.
        /// </summary>
        ActivityID,

        /// <summary>
        /// ETW supports the ability to take events with another GUID called the related activity that is either causes or is caused by the current activity. This property returns that GUID (or Guid.Empty if the event has not related activity.
        /// </summary>
        RelatedActivityID,

        /// <summary>
        /// Event Providers can define a 'message' for each event that are meant for human consumption. FormattedMessage returns this string with the values of the payload filled in at the appropriate places. It will return null if the event provider did not define a 'message' for this event.
        /// </summary>
        FormattedMessage,

        /// <summary>
        /// An EventIndex is a integer that is guaranteed to be unique for this event over the entire log. Its primary purpose is to act as a key that allows side tables to be built up that allow value added processing to 'attach' additional data to this particular event unambiguously. This property is only set for ETLX file. For ETL or real time streams it returns 0. EventIndex is currently a 4 byte quantity. This does limit this property to 4Gig of events.
        /// </summary>
        EventIndex,

        /// <summary>
        /// The TraceEventSource associated with this event.
        /// </summary>
        Source,

        /// <summary>
        /// Returns true if this event is from a Classic (Pre-VISTA) provider
        /// </summary>
        IsClassicProvider,

        /// <summary>
        /// The size of the event-specific data payload. (see EventData) Normally this property is not used because some TraceEventParser has built a subclass of TraceEvent that parses the payload.
        /// </summary>
        EventDataLength,

        /// <summary>
        /// Returns an array of bytes representing the event-specific payload associated with the event. Normally this method is not used because some TraceEventParser has built a subclass of TraceEvent that parses the payload.
        /// </summary>
        EventData,

        /// <summary>
        /// Dumps a very verbose description of the event, including a dump of they payload bytes. It is in XML format. This is very useful in debugging (put it in a watch window) when parsers are not interpreting payloads properly.
        /// </summary>
        Dump,

        /// <summary>
        /// Write an XML representation to the stringBuilder sb and return it.
        /// </summary>
        Xml,

        /// <summary>
        /// Returns a string concatenated with the payload name and payload value.
        /// </summary>
        Payloads,

        /// <summary>
        /// Returns the friendly name corresponding to the provider name (or ID).
        /// </summary>
        ProviderFriendlyName,

    }

}
