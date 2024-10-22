from flask import Flask, render_template, request, jsonify, redirect, url_for, send_file
import os
import requests
import ssl
import logging
import base64

app = Flask(__name__)
UPLOAD_FOLDER = 'static/uploads'
RESULT_FOLDER = 'static/results'
os.makedirs(UPLOAD_FOLDER, exist_ok=True)
os.makedirs(RESULT_FOLDER, exist_ok=True)

app.config['UPLOAD_FOLDER'] = UPLOAD_FOLDER
app.config['RESULT_FOLDER'] = RESULT_FOLDER

@app.route('/')
def index():
    return render_template('index.html')

@app.route('/upload', methods=['POST'])
def upload_file():
    if 'file' not in request.files:
        return redirect(request.url)
    file = request.files['file']
    if file.filename == '':
        return redirect(request.url)
    if file:
        filename = os.path.join(app.config['UPLOAD_FOLDER'], file.filename)
        file.save(filename)
        # 發送文件到 Windows API 伺服器
        try:
            with open(filename, 'rb') as f:
                encoded_string = base64.b64encode(f.read()).decode()
            response = requests.post('http://140.123.105.135:5000/api/analyze', json={'file': encoded_string})
            if response.status_code == 200:
                response_data = response.json()
                colored_image_base64 = response_data.get('ColoredImage')
                if colored_image_base64:
                    with open(filename, 'wb') as f:
                        f.write(base64.b64decode(colored_image_base64))
                    return jsonify({"message": "File analyzed and replaced", "file_url": url_for('static', filename='uploads/' + file.filename)})
                else:
                    return jsonify({"error": "No colored image returned from Windows"}), 500
            else:
                return jsonify({"error": response.text}), response.status_code
        except Exception as e:
            print(f"Error: {str(e)}")
            return jsonify({"error": str(e)}), 500
    return redirect(url_for('index'))

if __name__ == '__main__':
    os.makedirs('static/uploads', exist_ok=True)
    context = ssl.SSLContext(ssl.PROTOCOL_TLS)
    context.load_cert_chain(certfile='cert.pem', keyfile='key.pem')
    app.run(host='0.0.0.0', port=5000, ssl_context=context, debug=True)
