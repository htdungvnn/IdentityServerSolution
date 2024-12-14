# Use official Node.js image to build Angular app
FROM node:18 AS build
WORKDIR /app
COPY ./ /app
RUN npm install
RUN npm run build --configuration production

# Use Nginx to serve the built Angular files
FROM nginx:latest
COPY --from=build /app/dist/angular-spa /usr/share/nginx/html
EXPOSE 80
