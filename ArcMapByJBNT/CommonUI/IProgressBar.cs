using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ArcMapByJBNT
{
    /// <summary>
    /// 进度条控件接口
    /// </summary>
    public interface IProgressBar
    {
        int Maximum
        {
            get;
            set;
        }

        int Minimum
        {
            get;
            set;
        }

        int Step
        {
            get;
            set;
        }

        int Value
        {
            get;
            set;
        }

        ProgressBarStyle Style
        {
            get;
            set;
        }

        int MarqueeAnimationSpeed
        {
            get;
            set;
        }

        string Text
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 进度条窗体帮助接口
    /// </summary>
    public interface IProgressBarHelper : IProgressBar
    {
        /// <summary>
        /// 显示进度条消息
        /// </summary>
        string LineText { get; set; }

        /// <summary>
        /// 显示进度条窗体标题
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// 设置窗体是否可见
        /// </summary>
        /// <param name="Visible"></param>
        void SetVisible(bool Visible);

        /// <summary>
        /// 设置窗体内[取消]按钮是否可见
        /// </summary>
        /// <param name="Visible"></param>
        void SetIsCancel(bool Visible);

        /// <summary>
        /// 设置窗体内时间信息是否可见
        /// </summary>
        /// <param name="Visible"></param>
        void SetIsResidual(bool Visible);

        /// <summary>
        /// 设置窗体中的线程是否继续运行
        /// </summary>
        /// <param name="Running"></param>
        void SetIsRunning(bool Running);

        /// <summary>
        ///  显示进度条窗体 模态
        /// </summary>
        /// <param name="Parentonwer"></param>
        void ShowDialogHelper(IWin32Window Parentonwer);

        /// <summary>
        /// 显示进度条窗体 非模态
        /// </summary>
        /// <param name="Parentonwer"></param>
        void ShowHelper(IWin32Window Parentonwer);

        /// <summary>
        /// 隐藏进度条窗体
        /// </summary>
        void HideHelper();

        /// <summary>
        /// 关闭进度条窗体
        /// </summary>
        void CloseHelper();

        /// <summary>
        /// 注销进度条窗体
        /// </summary>
        void DisposeHelper();
    }

    /// <summary>
    /// 进度条工厂接口
    /// </summary>
    public interface IProgressBarFactory
    {
        /// <summary>
        /// 创建一个新进度条窗体
        /// </summary>
        /// <returns></returns>
        IProgressBarHelper Create();
    }
}
