FROM wordpress:6.8.2

RUN apt-get update
RUN apt-get install -y imagemagick
