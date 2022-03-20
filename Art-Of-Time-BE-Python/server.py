from concurrent import futures
import base64, grpc, time, subprocess


# import the generated classes
import imageGenerator_pb2
import imageGenerator_pb2_grpc


# based on .proto service
class ImageGeneratorServicer(imageGenerator_pb2_grpc.ImageGeneratorServicer):

    def GenerateImage(self, request, context):
        subprocess.call(['python3', './ArtOfTimeAI/generateImage.py', request.ImageId, request.BasedOnText, '&'])
        return imageGenerator_pb2.Empty

    def GetImage(self, request, context):
        response = imageGenerator_pb2.GeneratedImageResponse()
        with open(f"./steps/{request.ImageId}.png", "rb") as image_file:
            response.ImageBase64 = base64.b64encode(image_file.read()).decode('utf8').replace("'", '"')
            return response


# create a gRPC server
server = grpc.server(futures.ThreadPoolExecutor(max_workers=12))


# add the defined class to the server
imageGenerator_pb2_grpc.add_ImageGeneratorServicer_to_server(
        ImageGeneratorServicer(), server)

# listen on port 5000
print('Starting server. Listening on port 5000.')
server.add_insecure_port('[::]:5000')
server.start()

try:
    while True:
        time.sleep(5)
except KeyboardInterrupt:
    server.stop(0)