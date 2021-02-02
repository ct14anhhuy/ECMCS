using log4net.Layout;
using log4net.Util;

namespace ECMCS.App.Extension.log4net
{
    public class CustomPatternLayout : PatternLayout
    {
        public CustomPatternLayout()
        {
            AddConverter(new ConverterInfo { Name = "encodedmessage", Type = typeof(EncodedMessagePatternConvertor) });
        }
    }
}