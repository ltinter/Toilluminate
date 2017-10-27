using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToilluminateClient
{
    public static class PlayApp
    {
        public static List<PlayItem> PlayItemList = new List<PlayItem>();

        public static int CurrentIndex = -1;

        public static void Clear()
        {
            PlayItemList.Clear();
            CurrentIndex = -1;
        }

        public static PlayItem CurrentPlayItem()
        {
            if (CurrentIndex >= 0)
            {
                return PlayItemList[CurrentIndex];
            }
            else
            {
                return null;
            }
        }
    }

    public class PlayItem
    {
        #region " variable "

        private ShowPlayType playTypeValue = ShowPlayType.Image;

        private List<TempleteItem> templeteItemListValue = new List<TempleteItem>();

        private int currentTempleteItemIndex = -1;


        /// <summary>
        /// 开始时间
        /// </summary>
        private DateTime dtStartValue = DateTime.Now;

        /// <summary>
        /// 结束时间
        /// </summary>
        private DateTime dtEndValue = DateTime.Now;


        /// <summary>
        /// 循环次数
        /// </summary>
        private int cycleNumberValue = 0;
        /// <summary>
        /// 循环总数
        /// </summary>
        private int cycleTotalNumberValue = 1;

        /// <summary>
        /// 结束时间有效
        /// </summary>
        private bool endDateTimeValidValue = true;
        /// <summary>
        /// 循环总数有效
        /// </summary>
        private bool cycleTotalNumberValidValue = true;
        #endregion


        #region " propert "

        public ShowPlayType PlayType
        {
            get
            {
                return playTypeValue;
            }
            set
            {
                playTypeValue = value;
            }
        }
        public List<TempleteItem> TempleteItemList
        {
            get
            {
                return templeteItemListValue;
            }
        }
        public int CurrentTempleteIndex
        {
            get
            {
                return currentTempleteItemIndex;
            }
            set
            {
                currentTempleteItemIndex = value;
            }
        }
        public TempleteItem CurrentTempleteItem
        {
            get
            {
                if (currentTempleteItemIndex >= 0)
                {
                    return templeteItemListValue[currentTempleteItemIndex];
                }
                else
                {
                    return null;
                }
            }
        }



        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime DateTimeStart
        {
            get
            {
                return dtStartValue;
            }
            set
            {
                dtStartValue = value;
            }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime DateTimeEnd
        {
            get
            {
                return dtEndValue;
            }
            set
            {
                dtEndValue = value;
            }
        }


        /// <summary>
        /// 循环次数
        /// </summary>
        public int CycleNumber
        {
            get
            {
                return cycleNumberValue;
            }
            set
            {
                cycleNumberValue = value;
            }
        }

        /// <summary>
        /// 循环总数
        /// </summary>
        public int CycleTotalNumber
        {
            get
            {
                return cycleTotalNumberValue;
            }
            set
            {
                cycleTotalNumberValue = value;
            }
        }


        /// <summary>
        /// 结束时间有效
        /// </summary>
        public bool EndDateTimeValid
        {
            get
            {
                return endDateTimeValidValue;
            }
            set
            {
                endDateTimeValidValue = value;
            }
        }


        /// <summary>
        /// 循环总数有效
        /// </summary>
        public bool CycleTotalNumberValid
        {
            get
            {
                return cycleTotalNumberValidValue;
            }
            set
            {
                cycleTotalNumberValidValue = value;
            }
        }
        #endregion


        #region " Init "

        public PlayItem()
        {
           
        }
        public PlayItem(ShowPlayType playType)
        {
            playTypeValue = playType;
        }

        public PlayItem(ShowPlayType playType, string dtStart, string dtEnd, int cycleTotalNumber)
        {
            playTypeValue = playType;
            Utility.IsDateTime(dtStart, out dtStartValue);
            
            if (Utility.IsDateTime(dtEnd, out dtEndValue))
            {
                this.endDateTimeValidValue = true;
            }
            else
            {
                this.endDateTimeValidValue = false;
            }


            cycleTotalNumberValue = cycleTotalNumber;
            if (cycleTotalNumberValue > 0)
            {
                this.cycleTotalNumberValidValue = true;
            }else
            {
                this.cycleTotalNumberValidValue = false;
            }
        }
        #endregion

        #region " void and function "

        #endregion
    }


    public class TempleteItem
    {

        #region " variable "

        private string fileValue = string.Empty;

        private string messageValue = string.Empty;
        #endregion


        #region " propert "

        public string File
        {
            get
            {
                return fileValue;
            }
            set
            {
                fileValue = value;
            }
        }

        public string Message
        {
            get
            {
                return messageValue;
            }
            set
            {
                messageValue = value;
            }
        }
        #endregion


        public TempleteItem(string file ,string message)
        {
            fileValue = file;
            messageValue = message;
        }


        #region " void and function "

        #endregion
    }

    #region play类型
    /// <summary>
    /// play类型
    /// </summary>
    /// <remarks></remarks>
    public enum ShowPlayType
    {
        /// <summary>
        /// 图片
        /// </summary>
        [EnumDescription("写真")]
        Image = 0,
        /// <summary>
        /// 文字消息
        /// </summary>
        [EnumDescription("文字")]
        Message = 1,
        /// <summary>
        /// 视频
        /// </summary>
        [EnumDescription("映像")]
        Media = 2,
    }

    #endregion


    #region 模板类型
    /// <summary>
    /// 模板类型
    /// </summary>
    /// <remarks></remarks>
    public enum ShowTempleteType
    {
        /// <summary>
        /// 图片
        /// </summary>
        [EnumDescription("写真")]
        Image = 0,
        /// <summary>
        /// 文字消息
        /// </summary>
        [EnumDescription("文字")]
        Message = 1,
        /// <summary>
        /// 视频
        /// </summary>
        [EnumDescription("映像")]
        Media = 2,
    }

    #endregion

}
