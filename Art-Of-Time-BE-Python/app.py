import json
from flask import Flask, request
import base64, json, subprocess

app = Flask(__name__)

@app.route("/generate", methods=['POST'])
def generateImage():
    subprocess.call(['python3', './ArtOfTimeAI/generateImage.py', request.json['ImageId'],request.json['BasedOnText'], '&'])
    return "<p>Hello, World!</p>"

@app.route("/generate/<id>", methods=['GET'])
def getImage(id):
    with open(f"./steps/{id}.png", "rb") as image_file:
        return json.dumps({ "imageBase64": base64.b64encode(image_file.read()).decode('utf8').replace("'", '"') })
