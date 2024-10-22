@echo off

del .\.vs\*.* /q /s
rd .\.vs /q /s

del .\x64\*.* /q /s
rd .\x64 /q /s

del .\Win32\*.* /q /s
rd .\Win32 /q /s

del .\HSEnvPredict\obj\*.* /q /s
rd .\HSEnvPredict\obj /q /s
del .\HSEnvPredict\bin\x86\*.* /q /s
rd .\HSEnvPredict\bin\x86 /q /s
del .\HSEnvPredict\bin\x64\Release\*.pdb /q /s
del .\HSEnvPredict\bin\x64\Release\*.xml /q /s
del .\HSEnvPredict\bin\x64\Debug\*.pdb /q /s
del .\HSEnvPredict\bin\x64\Debug\*.xml /q /s

exit