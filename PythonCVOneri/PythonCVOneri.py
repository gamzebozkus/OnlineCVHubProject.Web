from flask import Flask, jsonify, request
import numpy as np
import pandas as pd
import pyodbc
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.metrics.pairwise import cosine_similarity

app = Flask(__name__)

def oneri_sistemi(aranacakTag, title):
    server = 'DESKTOP-2C6CDVI'
    database = 'OnlineCvHub'
    
    conn = pyodbc.connect('DRIVER={ODBC Driver 17 for SQL Server};'
                          'SERVER=' + server + ';'
                          'DATABASE=' + database + ';'
                          'Trusted_Connection=yes;')


    query = """
    SELECT *
    FROM UserCVs 
    INNER JOIN CvPools ON UserCVs.CVId = CvPools.CVId;
    """
    
    df = pd.read_sql(query, conn)
    conn.close()
    
    df = df[df["Title"].str.lower() == title]

    Tags = df["Tags"]
    metinler = df['Tags'].tolist() + [aranacakTag]

    vectorizer = TfidfVectorizer()
    tfidf_matrix = vectorizer.fit_transform(metinler)

    cos_sim = cosine_similarity(tfidf_matrix[-1], tfidf_matrix[:-1])

    df['similarity'] = cos_sim[0]
    sorted_df = df.sort_values(by='similarity', ascending=False)
    
    return sorted_df[['CVId', 'similarity']]

@app.route('/tavsiye', methods=['GET'])
def tavsiye():
    aranacakTag = request.args.get('aranacakTag')
    title = request.args.get('title').lower() if request.args.get('title') else "web developer"
    
    if not aranacakTag:
        return jsonify({'error': 'Lutfen bir aranacak tag giriniz.'}), 400
    
    sonuclar = oneri_sistemi(aranacakTag, title)
    return jsonify(sonuclar.to_dict(orient='records'))

if __name__ == '__main__':
    app.run(debug=True)
