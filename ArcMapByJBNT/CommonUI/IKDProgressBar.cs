/*
 * vp:hsg
 * date:2006-12-13
 * function:IKDProgressBar
 */
using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

namespace ArcMapByJBNT
{
    public interface  IKDProgressBar
    {
        string KDTitle { get;set;}
        void KDShow();
        void KDClose();
        void KDHide();
        void KDRefresh();
    }

    public interface IKDProgressBar1 : IKDProgressBar
    {
        string KDCaption1 { get;set;}
        System.Windows.Forms.ProgressBar KDProgressBar1 { get;set;}
        
    }
    public interface IKDProgressBar2 : IKDProgressBar1
    {
        string KDCaption2 { get;set;}
        System.Windows.Forms.ProgressBar KDProgressBar2 { get;set;}

    }
    public interface IKDProgressBar3 : IKDProgressBar2
    {
        string KDCaption3 { get;set;}
        System.Windows.Forms.ProgressBar KDProgressBar3 { get;set;}

    }
    public interface IKDProgressBar4 : IKDProgressBar3
    {
        string KDCaption4 { get;set;}
        System.Windows.Forms.ProgressBar KDProgressBar4 { get;set;}

    }
    public sealed class KDProgressBarFactory
    {
        public static IKDProgressBar getInstance(int BarNum)
        {
            IKDProgressBar tppb = null;
            switch (BarNum)
            {
                case 1:
                    tppb = new frmProgressBar1();
                    break;
                case 2:
                    tppb = new frmProgressBar2();
                    break;
                case 3:
                    tppb = new frmProgressBar3();
                    break;
                case 4:
                    tppb = new frmProgressBar4();
                    break;
                default:
                    tppb = new frmProgressBar4();
                    break;
            }
            return tppb;
        }
    }


}
