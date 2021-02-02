using log4net.Core;
using log4net.Util;
using System.IO;

namespace ECMCS.App.Extension.log4net
{
    public class EncodedMessagePatternConvertor : PatternConverter
    {
        protected override void Convert(TextWriter writer, object state)
        {
            var loggingEvent = state as LoggingEvent;
            if (loggingEvent == null)
            {
                return;
            }
            var encodedMessage = loggingEvent.RenderedMessage.Replace("\r", " ").Replace("\n", " ");
            writer.Write(encodedMessage);
        }
    }
}