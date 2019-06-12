Summary
Gen_AuditTrail is a python application to extract Audit Trail data, render HTML to format data,  and generate PDF based on rendered HTML. The resulting pdf file is saved to "output" folder.

Dependencies
1) wkhtmltopdf is the main pdf generation library that must be installed on the server. Current installation is located "C:\Program Files\wkhtmltopdf". You can search and download wkhtmltopdf if you need to reinstall/migrate this application. After installing wkhtmltopdf, you must add the bin directory into system PATH variable.

2) output folder must exist. 

3)config.json is the config file and stores the Audit trail type 1 and 2. Query updates should be made in this file.

How To Run
1) Open command prompt and navigate to the directory where "Gen_AuditTrail.exe" is located. Run the executeable in the command prompt, "Gen_AuditTrail.exe {companyid} {Audit Type}". eg: "Gen_AuditTrail.exe 62 2".