@echo off

%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild SpkRepo.sln /p:Configuration=Release /t:Rebuild /property:VisualStudioVersion=12.0

utils\7za a -ttar artifacts\package.tar .\SpkRepo.SelfHost\bin\Release\*
utils\7za a -tgzip artifacts\package.tgz .\artifacts\package.tar

del artifacts\spkrepo.spk 

utils\7za a -ttar artifacts\spkrepo.spk .\artifacts\package.tgz
utils\7za a -ttar artifacts\spkrepo.spk .\package\*

del artifacts\package.tar
del artifacts\package.tgz