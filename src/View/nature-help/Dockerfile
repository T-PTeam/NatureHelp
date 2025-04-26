# Stage 1: Build the Angular application
FROM node:20 AS frontend-build

# Install bash (тільки якщо потрібно)
RUN apt-get update && apt-get install -y bash

# Setup the working directory
WORKDIR /usr/src/app

# Copy package files
COPY package.json package-lock.json ./

# Install Angular CLI globally (опційно)
RUN npm install -g @angular/cli

# Install dependencies
RUN npm install --legacy-peer-deps

# Copy the rest of the app
COPY . .

# Build Angular app with українською локалізацією
RUN npm run build -- --configuration=production --localize --base-href /nature-help/

FROM nginx:alpine AS frontend

# Копіюємо білд із першого етапу
COPY --from=frontend-build /usr/src/app/dist/ /usr/share/nginx/html/

COPY nginx.conf /etc/nginx/conf.d/default.conf
# Відкриваємо порт 8080 (або який тобі потрібно)
EXPOSE 8080

# Запускаємо сервер

CMD ["http-server", ".", "-p", "8080"]

CMD ["nginx", "-g", "daemon off;"]