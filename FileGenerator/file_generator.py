'''2/25/2019 kh - Built this app to generate reports, data files that
will be sent back to client via
email, datafeed, or api. Having this app, negate the need to get DBA involved
to create SSIS job for us.
version 0.1 - pre-release version to test out reliability. This will be running
Bofa CSDB files during testing period.'''
import json
import pandas as pd
import sys 
import pyodbc
from datetime import datetime

with open('config.json') as json_data_file:
    DATA = json.load(json_data_file)
    DRIVER = 'Driver=' + DATA['i9tracker']['driver'] +';'
    HOST = 'Server=' + DATA['i9tracker']['host'] + ';'
    USER = 'UID=' + DATA['i9tracker']['user'] + ';'
    PWD = 'PWD=' + DATA['i9tracker']['pwd'] +';'

def main():
    '''
    2/22/2019 kh - main function within file generator.
    '''
    i9staging_string = DRIVER + HOST + 'Database=I9Staging;' + USER + PWD
    i9tracker_string = DRIVER + HOST + 'Database=I9Tracker;' + USER + PWD
    query = 'select Query,FileNameSchema, FileType from FileGenerators Where filegeneratorid='

    fid = sys.argv[1]
    print("Pulling query to generate file.")
    #get the query that will be executed to generate file.
    try:
        conn_query = pyodbc.connect(i9staging_string)
        cursor = conn_query.cursor()
        cursor.execute(query + str(fid))
    except:
        print('Error with establising database connection!')
    else:
        row = cursor.fetchone()
        file_query = row[0]
        file_name_schema = row[1]
        file_type = row[2]
    finally:
        conn_query.close()

    #execute the query to generate data, that will be written into a file.
    print("Executing query.")
    try:
        #if txt file then just create it normally.
        if file_type == 'txt':
            conn = pyodbc.connect(i9tracker_string)
            csr = conn.cursor()
            csr.execute(file_query)
            fg_file = open(f'{file_name_schema}', 'w')
            for row in csr:
                fg_file.writelines(row[0] + '\n')

            fg_file.close()
        elif file_type =='xlsx':
            #conn.close()
            conn = pyodbc.connect(i9tracker_string)
            xslx(file_name_schema, file_query, conn)
    except:
        print('Error with establising database conn!')
        
    finally:
        conn.close()
        


def xslx(fname, fquery, fconn):
    '''
    function used to execute query, and generate excel spreadsheet using results.
    '''
    # Create a Pandas dataframe from some data.
    df = pd.read_sql(fquery, fconn)
    print("Generating excel file.")
    # Create a Pandas Excel writer using XlsxWriter as the engine.
    writer = pd.ExcelWriter(InsertPlaceholder(fname), engine='xlsxwriter')

    # Convert the dataframe to an XlsxWriter Excel object.
    df.to_excel(writer, sheet_name='Sheet1', index=False)

    # Close the Pandas Excel writer and output the Excel file.
    writer.save()

def InsertPlaceholder(fname):
    now = datetime.now().strftime("%m-%d-%Y")
    return fname.replace('{DateStamp}', now)


if __name__ == '__main__':
    main()
