..\external-libs\WiX\candle abNORM-MergeModule.wxs
..\external-libs\WiX\light abNORM-MergeModule.wixobj -out abNORM-MergeModule.msm

..\external-libs\WiX\candle abNORM-Setup.wxs
..\external-libs\WiX\light abNORM-Setup.wixobj -out abNORM-Setup.msi ..\external-libs\WiX\wixui.wixlib -loc ..\external-libs\WiX\WixUI_en-us.wxl

del *.wixobj