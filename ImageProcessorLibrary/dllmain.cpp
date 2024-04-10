// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include <string>
#include <windows.h>
#include <opencv2/core.hpp>
#include <opencv2/opencv.hpp>
#include <opencv2/imgproc.hpp>
#include <vector>

extern "C" __declspec(dllexport) cv::Mat* blur_byte_img(unsigned char byteArr[], int length)
{
    //Decode the byte array into an OpenCV Mat object
    cv::Mat img = cv::imdecode(cv::Mat(1, length, CV_8UC1, byteArr), cv::IMREAD_UNCHANGED);
    //Optimize OpenCV usage for better performance
    cv::setUseOptimized(true);
    cv::setNumThreads(cv::getNumberOfCPUs());
    //Apply Gaussian blur to the image
    cv::GaussianBlur(img, img, cv::Size(15, 15), 0);

    //Return a pointer to the proccessed image
    return new cv::Mat(img);
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }

    return TRUE;
}

