// Atum.OperatorStation.cpp : Defines the entry point for the console application.

#include "stdafx.h"
#include <cstdlib>
#include <iostream>
#include <sstream>
#include <cassert>
#include <fstream>
#include "RawFiles.h"
#pragma region Includes and Imports
#include <windows.h>
#include <metahost.h>
#include <vector>

#pragma comment(lib, "mscoree.lib")
#import "C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.tlb" raw_interfaces_only\
            high_property_prefixes("_get","_put","_putref")\
            rename("ReportEvent", "InteropServices_ReportEvent")\
            exclude("ITrackingHandler")
using namespace mscorlib;

#pragma endregion
unsigned char decryptedData[RAW_ASSEMBLY_LENGTH];

int APIENTRY _tWinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPTSTR lpCmdLine, int nCmdShow)
{
    CoInitializeEx(NULL, COINIT_APARTMENTTHREADED);
    ICLRMetaHost       *pMetaHost = NULL;
    ICLRMetaHostPolicy *pMetaHostPolicy = NULL;
    ICLRDebugging      *pCLRDebugging = NULL;
    HRESULT hr;
    hr = CLRCreateInstance(CLSID_CLRMetaHost, IID_ICLRMetaHost,
        (LPVOID*)&pMetaHost);
    hr = CLRCreateInstance(CLSID_CLRMetaHostPolicy, IID_ICLRMetaHostPolicy,
        (LPVOID*)&pMetaHostPolicy);
    hr = CLRCreateInstance(CLSID_CLRDebugging, IID_ICLRDebugging,
        (LPVOID*)&pCLRDebugging);
    ICLRRuntimeInfo* pRuntimeInfo = NULL;
    hr = pMetaHost->GetRuntime(L"v4.0.30319", IID_ICLRRuntimeInfo, (VOID**)&pRuntimeInfo);
    BOOL bLoadable;
    hr = pRuntimeInfo->IsLoadable(&bLoadable);
    ICorRuntimeHost* pRuntimeHost = NULL;
    hr = pRuntimeInfo->GetInterface(CLSID_CorRuntimeHost,
        IID_ICorRuntimeHost,
        (VOID**)&pRuntimeHost);
    hr = pRuntimeHost->Start();
    IUnknownPtr pAppDomainThunk = NULL;
    hr = pRuntimeHost->GetDefaultDomain(&pAppDomainThunk);
    _AppDomainPtr pDefaultAppDomain = NULL;
    hr = pAppDomainThunk->QueryInterface(__uuidof(_AppDomain), (VOID**)&pDefaultAppDomain);
    _AssemblyPtr pAssembly = NULL;

    SAFEARRAYBOUND rgsabound[1];
    rgsabound[0].cElements = RAW_ASSEMBLY_LENGTH;
    rgsabound[0].lLbound = 0;
    SAFEARRAY* pSafeArray = SafeArrayCreate(VT_UI1, 1, rgsabound);
    void* pvData = NULL;
    hr = SafeArrayAccessData(pSafeArray, &pvData);
    for (size_t i = 0; i < RAW_ASSEMBLY_LENGTH; i++)
    {
        // decryptedData[i] = ~rawData[i];
        decryptedData[i] = rawData[i] ^ 'H';
    }

    memcpy(pvData, decryptedData, RAW_ASSEMBLY_LENGTH);
    // memcpy(pvData, rawData, RAW_ASSEMBLY_LENGTH);

    hr = SafeArrayUnaccessData(pSafeArray);
    hr = pDefaultAppDomain->Load_3(pSafeArray, &pAssembly);
    _MethodInfoPtr pMethodInfo = NULL;
    hr = pAssembly->get_EntryPoint(&pMethodInfo);
    VARIANT retVal;
    ZeroMemory(&retVal, sizeof(VARIANT));
    VARIANT obj;
    ZeroMemory(&obj, sizeof(VARIANT));
    obj.vt = VT_NULL;
    SAFEARRAY *psaStaticMethodArgs = SafeArrayCreateVector(VT_VARIANT, 0, 0);
    hr = pMethodInfo->Invoke_3(obj, psaStaticMethodArgs, &retVal);

    return 0;
}