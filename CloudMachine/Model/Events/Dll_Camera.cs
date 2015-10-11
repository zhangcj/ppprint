﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime;
using System.Runtime.InteropServices;
namespace CloudMachine.Model.Events
{
    class Dll_Camera
    {
        //   add by zcj     参数：HWND hwnd; 接收解码信息 的窗口句柄。
        //函数功能：将接收解码信息 的窗口句柄传给dll

        [DllImport("dll_camera.dll", EntryPoint = "GetAppHandle",CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetAppHandle(IntPtr hWnd);
        //函数功能：启动设备。
        //返回值：true 设备启动成功
        //false设备启动失败

        [DllImport("dll_camera.dll", EntryPoint = "StartDevice",CallingConvention = CallingConvention.Cdecl)]
        public static extern int StartDevice();
        //释放摄像头。
        [DllImport("dll_camera.dll", EntryPoint = "ReleaseDevice",CallingConvention = CallingConvention.Cdecl)]
        public static extern void ReleaseDevice();
        //参数：bqr， true时，qr引擎开启；false时qr引擎关闭
        //函数功能：设置QR引擎状态
        [DllImport("dll_camera.dll", EntryPoint = "setQRable",CallingConvention = CallingConvention.Cdecl)]
        public static extern void setQRable(bool bqr);
        //参数：bdm ，true时，dm引擎开启；false时dm引擎关闭
        //函数功能：设置DM引擎状态
        [DllImport("dll_camera.dll", EntryPoint = "setDMable",CallingConvention = CallingConvention.Cdecl)]
        public static extern void setDMable(bool bdm);
        //        参数：bbarcode， true时，一维引擎开启；false时一维引擎关闭
        //函数功能：设置一维引擎状态
        [DllImport("dll_camera.dll", EntryPoint = "setBarcode",CallingConvention = CallingConvention.Cdecl)]
        public static extern void setBarcode(bool bbarcode);       

        //参数：int BeepTime  蜂鸣时间 单位：ms
        //函数功能：设定解码成功后的蜂鸣时间
        [DllImport("dll_camera.dll", EntryPoint = "SetBeepTime",CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetBeepTime(int BeepTime);

        //参数：char *aa，获取解码信息的指针
        //函数功能：获取解码信息
        [DllImport("dll_camera.dll", EntryPoint = "GetDecodeString",CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetDecodeString(StringBuilder sb, out int len);

        //函数功能：查找设备
        //返回值：1，表示查找设备成功
        //        -1，表示查找设备失败。
        [DllImport("dll_camera.dll", EntryPoint = "GetDevice",CallingConvention = CallingConvention.Cdecl)]
        public static extern int  GetDevice();

        //函数功能：人为拔出设备时，需要释放的摄像头资源。
        //返回值：无
        [DllImport("dll_camera.dll", EntryPoint = "ReleaseLostDevice", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ReleaseLostDevice();
       
    }
}
