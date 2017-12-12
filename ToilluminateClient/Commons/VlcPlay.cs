using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ToilluminateClient
{
    public delegate void VLCPlayerStopEventHandler(object sender, EventArgs e);



    public class VLCPlayer
    {
        private IntPtr libVLCInstance;
        private IntPtr libVLCMediaPlayer;

        private double duration;

        private VLCPlayState playState = VLCPlayState.STATE_Stop;

        public VLCPlayer()
        {
            string pluginPath = Environment.CurrentDirectory + "\\plugins\\";  //插件目录
            string plugin_arg = "--plugin-path=" + pluginPath;
            string[] arguments = { "-I", "dummy", "--ignore-config", "--no-video-title", plugin_arg, "--avcodec-hw=dxva2" };

            libVLCInstance = VLCAPI.libvlc_new(arguments);

            //创建 libVLCMediaPlayer 播放核心
            if (libVLCInstance != IntPtr.Zero)
                libVLCMediaPlayer = VLCAPI.libvlc_media_player_new(libVLCInstance);

        }
        public void Dispose()
        {
            if (libVLCInstance != IntPtr.Zero)
                VLCAPI.libvlc_release(libVLCInstance);
        }

        public event VLCPlayerStopEventHandler OnStopEvent;

        protected virtual void StopEvent(EventArgs e)
        {
            if (OnStopEvent != null)
            {
                OnStopEvent(this, e);
            }
        }



        /// <summary>
        /// 设置播放容器
        /// </summary>
        /// <param name="wndHandle">播放容器句柄</param>
        public void SetRenderWindow(IntPtr wndHandle)
        {
            SetRenderWindow((int)wndHandle);
        }
        public void SetRenderWindow(int wndHandle)
        {
            if (libVLCInstance != IntPtr.Zero && wndHandle != 0)
            {
                VLCAPI.libvlc_media_player_set_hwnd(libVLCMediaPlayer, wndHandle);
            }
        }

        /// <summary>
        /// 播放指定媒体文件
        /// </summary>
        /// <param name="filePath"></param>
        public void LoadFile(string filePath)
        {
            IntPtr libvlc_media = VLCAPI.libvlc_media_new_path(libVLCInstance, filePath);  //创建 libVLCMediaPlayer 播放核心
            if (libvlc_media != IntPtr.Zero)
            {
                VLCAPI.libvlc_media_parse(libvlc_media);
                duration = VLCAPI.libvlc_media_get_duration(libvlc_media) / 1000.0;  //获取视频时长

                VLCAPI.libvlc_media_player_set_media(libVLCMediaPlayer, libvlc_media);  //将视频绑定到播放器去
                VLCAPI.libvlc_media_release(libvlc_media);

                //VLCAPI.libvlc_media_player_play(libVLCMediaPlayer);  //播放
            }
        }
        /// <summary>
        /// 播放
        /// </summary>
        public void Play()
        {
            if (libVLCMediaPlayer != IntPtr.Zero)
            {
                if (playState != VLCPlayState.STATE_Play)
                {
                    VLCAPI.libvlc_media_player_play(libVLCMediaPlayer);
                    playState = VLCPlayState.STATE_Play;
                }
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {

            if (libVLCMediaPlayer != IntPtr.Zero)
            {
                VLCAPI.libvlc_media_player_pause(libVLCMediaPlayer);
                playState = VLCPlayState.STATE_Pause;
            }

        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            if (libVLCMediaPlayer != IntPtr.Zero)
            {
                VLCAPI.libvlc_media_player_stop(libVLCMediaPlayer);
                playState = VLCPlayState.STATE_Stop;
                StopEvent(EventArgs.Empty);
            }
        }

        public void Release()
        {
            if (libVLCMediaPlayer != IntPtr.Zero)
            {
                VLCAPI.libvlc_media_release(libVLCMediaPlayer);
            }
        }
        /// <summary>
        /// 播放时间
        /// </summary>
        public double PlayTime
        {
            get
            {
                if (libVLCMediaPlayer == IntPtr.Zero) return 0;
                return VLCAPI.libvlc_media_player_get_time(libVLCMediaPlayer) / 1000.0;
            }
            set
            {
                if (libVLCMediaPlayer != IntPtr.Zero)
                    VLCAPI.libvlc_media_player_set_time(libVLCMediaPlayer, (Int64)(value * 1000));
            }
        }


        /// <summary>
        /// 音量
        /// </summary>
        public int Volume
        {
            get
            {
                if (libVLCMediaPlayer != IntPtr.Zero)
                    return VLCAPI.libvlc_audio_get_volume(libVLCMediaPlayer);
                else
                    return -1;
            }
            set
            {
                if (libVLCMediaPlayer != IntPtr.Zero)
                    VLCAPI.libvlc_audio_set_volume(libVLCMediaPlayer, value);
            }
        }


        /// <summary>
        /// 设置是否全屏
        /// </summary>
        public void SetFullScreen(bool istrue)
        {
            if (libVLCMediaPlayer != IntPtr.Zero)
                VLCAPI.libvlc_set_fullscreen(libVLCMediaPlayer, istrue ? 1 : 0);

        }

        /// <summary>
        /// 视频时长
        /// </summary>
        public double Duration
        {
            get
            {
                return duration;
            }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public VLCPlayState State
        {
            get
            {
                return playState;
            }
        }

        /// <summary>
        /// 是否正在播放
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                if (playState != VLCPlayState.STATE_Stop)
                {
                    libvlc_state_t state= VLCAPI.libvlc_media_player_get_state(libVLCMediaPlayer);
                    if (state == libvlc_state_t.libvlc_Ended)
                    {
                        this.Stop();  //如果播放完，关闭视频
                    }
                }
               

                bool isPlaying = this.playState == VLCPlayState.STATE_Play;

                return isPlaying;
            }
        }

        public string Version()
        {
            return VLCAPI.libvlc_get_version();
        }

        /// <summary>
        /// 抓图(支持多种格式)
        /// </summary>
        /// <param name="savePath">完整路径（bmp,png,jpg）</param>
        /// <param name="width">抓图宽度</param>
        /// <param name="height">抓图高度</param>
        /// <returns></returns>
        public int SnapShot(string savePath, uint width = 0, uint height = 0)
        {
            int result = -1;
            if (libVLCMediaPlayer != IntPtr.Zero)
            {
                IntPtr pathPtr = VLCUtil.StringToPtr(savePath);
                result = VLCAPI.libvlc_video_take_snapshot(libVLCMediaPlayer, 0, pathPtr, width, height);
                Marshal.FreeHGlobal(pathPtr);

            }
            return result;
        }
    }
    internal static class VLCAPI
    {
        internal struct PointerToArrayOfPointerHelper
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
            public IntPtr[] pointers;
        }

        /// <summary>
        /// 传入播放参数
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static IntPtr libvlc_new(string[] arguments)
        {
            PointerToArrayOfPointerHelper argv = new PointerToArrayOfPointerHelper();
            argv.pointers = new IntPtr[11];

            for (int i = 0; i < arguments.Length; i++)
            {
                argv.pointers[i] = Marshal.StringToHGlobalAnsi(arguments[i]);  //将托管 System.String 中的内容复制到非托管内存，并在复制时转换为 ANSI 格式。
            }

            IntPtr argvPtr = IntPtr.Zero;
            try
            {
                int size = Marshal.SizeOf(typeof(PointerToArrayOfPointerHelper));  //返回非托管类型的大小（以字节为单位）。
                argvPtr = Marshal.AllocHGlobal(size);  //从进程的非托管内存中分配内存。
                Marshal.StructureToPtr(argv, argvPtr, false);  //将数据从托管对象封送到非托管内存块。

                return libvlc_new(arguments.Length, argvPtr);  //创建一个libvlc实例，它是引用计数的
            }
            finally
            {
                for (int i = 0; i < arguments.Length + 1; i++)
                {
                    if (argv.pointers[i] != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(argv.pointers[i]);  //释放以前使用 System.Runtime.InteropServices.Marshal.AllocHGlobal(System.IntPtr) 从进程的非托管内存中分配的内存。
                    }
                }

                if (argvPtr != IntPtr.Zero) { Marshal.FreeHGlobal(argvPtr);/* 释放以前使用 System.Runtime.InteropServices.Marshal.AllocHGlobal(System.IntPtr) 从进程的非托管内存中分配的内存。 */ }
            }
        }

        /// <summary>
        /// 从本地文件系统路径新建,其他参照上一条
        /// </summary>
        /// <param name="libVLCInstance"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IntPtr libvlc_media_new_path(IntPtr libVLCInstance, string path)
        {
            IntPtr pMrl = IntPtr.Zero;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(path);
                pMrl = Marshal.AllocHGlobal(bytes.Length + 1);
                Marshal.Copy(bytes, 0, pMrl, bytes.Length);
                Marshal.WriteByte(pMrl, bytes.Length, 0);
                return libvlc_media_new_path(libVLCInstance, pMrl);  // 从本地文件路径构建一个libvlc_media
            }
            finally
            {
                if (pMrl != IntPtr.Zero) { Marshal.FreeHGlobal(pMrl);/* 释放以前使用 System.Runtime.InteropServices.Marshal.AllocHGlobal(System.IntPtr) 从进程的非托管内存中分配的内存。 */                }
            }
        }

        /// <summary>
        /// 使用一个给定的媒体资源路径来建立一个libvlc_media对象.参数psz_mrl为要读取的MRL(Media Resource Location).此函数返回新建的对象或NULL.
        /// </summary>
        /// <param name="libVLCInstance"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IntPtr libvlc_media_new_location(IntPtr libVLCInstance, string path)
        {
            IntPtr pMrl = IntPtr.Zero;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(path);
                pMrl = Marshal.AllocHGlobal(bytes.Length + 1);
                Marshal.Copy(bytes, 0, pMrl, bytes.Length);
                Marshal.WriteByte(pMrl, bytes.Length, 0);
                return libvlc_media_new_path(libVLCInstance, pMrl);  // 从本地文件路径构建一个libvlc_media
            }
            finally
            {
                if (pMrl != IntPtr.Zero) { Marshal.FreeHGlobal(pMrl);/* 释放以前使用 System.Runtime.InteropServices.Marshal.AllocHGlobal(System.IntPtr) 从进程的非托管内存中分配的内存。 */                }
            }
        }

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr libvlc_new(int argc, IntPtr argv);

        // 释放libvlc实例

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_release(IntPtr libVLCInstance);



        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern String libvlc_get_version();



        // 从视频来源(例如Url)构建一个libvlc_meida   RTSP

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr libvlc_media_new_location(IntPtr libVLCInstance, IntPtr path);



        // 从本地文件路径构建一个libvlc_media   rtsp串流不适合调用此接口
        // [MarshalAs(UnmanagedType.LPStr)] string  path
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr libvlc_media_new_path(IntPtr libVLCInstance, IntPtr path);

        /// <summary>
        /// 影片长度
        /// </summary>
        /// <param name="libVLCInstance"></param>
        /// <returns></returns>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr libvlc_media_player_get_length(IntPtr libVLCMediaPlayer);


        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_release(IntPtr libvlc_media_inst);



        // 创建libVLCMediaPlayer(播放核心)

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr libvlc_media_player_new(IntPtr libVLCInstance);



        // 将视频(libvlc_media)绑定到播放器上

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_set_media(IntPtr libVLCMediaPlayer, IntPtr libvlc_media);


        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_new_from_media(IntPtr libVLCMediaPlayer);

        // 设置图像输出的窗口

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_set_hwnd(IntPtr libvlc_mediaplayer, Int32 drawable);



        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_play(IntPtr libvlc_mediaplayer);



        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_pause(IntPtr libvlc_mediaplayer);



        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_stop(IntPtr libvlc_mediaplayer);



        // 解析视频资源的媒体信息(如时长等)

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_parse(IntPtr libvlc_media);



        // 返回视频的时长(必须先调用libvlc_media_parse之后，该函数才会生效)

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern Int64 libvlc_media_get_duration(IntPtr libvlc_media);



        // 当前播放的时间
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern Int64 libvlc_media_player_get_time(IntPtr libvlc_mediaplayer);



        // 设置播放位置(拖动)

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_set_time(IntPtr libvlc_mediaplayer, Int64 time);

        /// <summary>
        /// 抓图
        /// </summary>
        /// <param name="libvlc_mediaplayer"></param>
        /// <param name="num">经典0</param>
        /// <param name="filePath">完整路径，文件名英文或下划线开头</param>
        /// <param name="i_width"></param>
        /// <param name="i_height"></param>
        /// <returns></returns>
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int libvlc_video_take_snapshot(IntPtr libvlc_mediaplayer, uint num, IntPtr filePath, uint i_width, uint i_height);


        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_media_player_release(IntPtr libvlc_mediaplayer);



        // 获取和设置音量

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int libvlc_audio_get_volume(IntPtr libVLCMediaPlayer);



        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_audio_set_volume(IntPtr libVLCMediaPlayer, int volume);



        // 设置全屏

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void libvlc_set_fullscreen(IntPtr libVLCMediaPlayer, int isFullScreen);


        // 设置全屏

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern libvlc_state_t libvlc_media_player_get_state(IntPtr libVLCMediaPlayer);

    }

    static class VLCUtil
    {
        public static IntPtr ReturnIntPtr(byte[][] data, int length)
        {

            IntPtr[] dataIntPtrArr = new IntPtr[length];

            for (int i = 0; i < length; i++)
            {

                dataIntPtrArr[i] = Marshal.AllocHGlobal(data[i].Length * sizeof(byte));

                Marshal.Copy(data[i], 0, dataIntPtrArr[i], data[i].Length);

            }

            IntPtr dataIntPtr = Marshal.AllocHGlobal(length * Marshal.SizeOf(typeof(IntPtr)));

            Marshal.Copy(dataIntPtrArr, 0, dataIntPtr, length);

            return dataIntPtr;

        }
        /// <summary>
        /// 字符串转指针
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static IntPtr StringToPtr(string str)
        {
            byte[] bs = Encoding.UTF8.GetBytes(str);

            List<byte> list = new List<byte>(100);
            list.AddRange(bs);
            list.Add((byte)0);//必须末尾增加0，c语言字符串以0结束，否则文件名乱码

            IntPtr pathPtr = Marshal.AllocHGlobal(list.Count);

            Marshal.Copy(list.ToArray(), 0, pathPtr, list.Count);
            return pathPtr;
        }

    }
    public enum VLCPlayState
    {
        /// <summary>
        /// 
        /// </summary>
        STATE_Stop = 0,
        /// <summary>
        /// 
        /// </summary>
        STATE_Play = 1,
        /// <summary>
        /// 
        /// </summary>
        STATE_Pause = 2,
    }
    public enum libvlc_state_t
    {
        libvlc_NothingSpecial = 0,
        libvlc_Opening,
        libvlc_Buffering,
        libvlc_Playing,
        libvlc_Paused,
        libvlc_Stopped,
        libvlc_Ended,
        libvlc_Error,
    }
}
