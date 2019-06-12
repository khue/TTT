'''
5/10/2019 kh - Purpose of this is to pull audit trails into a grid, then generate a PDF of that data.
1) Pull i9id from pdfexports table
2) Walk through each I9id, and run audit export script(s)
3) Resulting data place into a grid
4) Generate PDF and store somewhere
'''
import json
import sys 
import pyodbc
from jinja2 import Template
import pdfkit
from pathlib import Path
#path_wkthmltopdf = r'C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf.exe'
#config = pdfkit.configuration(wkhtmltopdf=path_wkthmltopdf)

#configure pdf output parameters
options = {
       'dpi': 400,
       'orientation': 'Landscape',
       'header-font-size': 10,
       'encoding': "UTF-8"
       
}

with open('config.json') as json_data_file:
    DATA = json.load(json_data_file)
    DRIVER = 'Driver=' + DATA['i9tracker']['driver'] +';'
    HOST = 'Server=' + DATA['i9tracker']['host'] + ';'
    USER = 'UID=' + DATA['i9tracker']['user'] + ';'
    PWD = 'PWD=' + DATA['i9tracker']['pwd'] +';'

AuditTrail_Type2 =  DATA['i9tracker']['Type2']
audit_template = DATA['i9tracker']['audit_template']
I9Query = ""
Fname = ""

def main():
    i9tracker_string = DRIVER + HOST + 'Database=I9Tracker;' + USER + PWD

    #get companyid and typeid from execution parameters
    cid = sys.argv[1]
    typeid = sys.argv[2]

    query = "select p.I9Id, e.Employeeid as FragId, e.FirstName, e.LastName, isnull(e.CompanyEmployeeId,'NoEEId') as EEID From PDFExports p with(nolock) inner join i9s with(nolock) on i9s.i9id=p.I9ID inner join employees e with(nolock) on e.employeeid=i9s.employeeid WHERE p.exportdate is null and CompanyID = " + cid 

    #get the list of I9s that we need to pull audit trail for
    try:
        conn_query = pyodbc.connect(i9tracker_string)
        cursor = conn_query.cursor()
        cursor.execute(query)
    except:
        print('Error with establising database connection!')
    else:
        rows = cursor.fetchall()
        #loop through each i9 to run audittrail query.
        for row in rows:
            I9Query = AuditTrail_Type2.replace('@I9Id', str(row.I9Id))
            #build the file name
            Fname = '_'.join([str(row.FragId), str(row.I9Id), str(row.EEID), row.FirstName, row.LastName, 'AuditTrail.pdf'])
            Fname = Path.cwd().joinpath('output').joinpath(Fname)

            conn = pyodbc.connect(i9tracker_string)
            csr = conn.cursor()
            csr.execute(I9Query)
            trail_rows = csr.fetchall()

            trail_data = []
            template = Template(audit_template)

            for trow in trail_rows:
                trail_data.append(trow)

            #print(Fname)                            
            pdfkit.from_string(template.render(trows=trail_data, i9id=str(row.I9Id)),str(Fname), options)
            
            
    finally:
        conn_query.close()


if __name__ == '__main__':
    main()

