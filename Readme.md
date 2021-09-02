#### Running Docker image
* The image in the DockerHub repository and can be found under the name: <code>heidelbergcementwebapi</code> by author<code>fundywave</code>
##### Automatic downloading and running the image
* Open <code>Docker_Image_Download_And_Run.cmd</code>
##### Manual downloading and running the image
* To download and run the container
In command prompt: 
<code>docker pull fundywave/heidelbergcementwebapi:latest</code>
* To run the continer: 
<code>docker run --rm -it  -p 5000:5000/tcp fundywave/heidelbergcementwebapi:latest</code>
##### Browsing
* In browser visit: <code>localhost:5000/api/logproxy</code>
* For basic auth:  <code>admin/123</code>
#### Compiling and Project
* Open command promt in the project folder and type:
<code>dotnet build</code>
then <code>dotnet run</code>
* Open the listening url in browser as <code>localhost:port/api/logproxy</code>
