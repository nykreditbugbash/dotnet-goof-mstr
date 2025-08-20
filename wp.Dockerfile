FROM wordpress:6.7.0

RUN apt-get update
RUN apt-get install -y imagemagick
