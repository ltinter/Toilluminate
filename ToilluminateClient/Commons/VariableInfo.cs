using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToilluminateClient
{
    public class VariableInfo
    {
        /// <summary>
        /// client path
        /// </summary>
        public static string ClientPath = string.Empty;
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ImageShowStyle
    {
        /// <summary>
        /// 
        /// </summary>
        None=0,
        /// <summary>
        /// 
        /// </summary>
        TopToDown = 1,
        /// <summary>
        /// 
        /// </summary>
        DownToTop = 2,
        /// <summary>
        /// 
        /// </summary>
        LeftToRight = 3,
        /// <summary>
        /// 
        /// </summary>
        RightToLeft = 4,
        /// <summary>
        /// 
        /// </summary>
        SmallToBig= 5,
        /// <summary>
        /// 
        /// </summary>
        Gradient = 6,

    }
}
