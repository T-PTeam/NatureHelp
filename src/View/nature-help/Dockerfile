FROM --platform=linux/amd64 node:20 AS frontend-build
WORKDIR /usr/src/app
ARG ENVIRONMENT_FILE=environment.docker.ts
ARG API_BASE_URL=
COPY package.json package-lock.json ./
RUN npm install -g @angular/cli
RUN npm install --legacy-peer-deps
COPY . .
COPY src/environments/${ENVIRONMENT_FILE} src/environments/environment.ts
RUN if [ -n "$API_BASE_URL" ]; then printf 'export const environment = {\n  apiUrl: "%s",\n};\n' "$API_BASE_URL" > src/environments/environment.ts; fi
# Creates: /usr/src/app/dist/nature-help/
RUN npm run build -- --configuration=production --base-href /nature-help/

FROM --platform=linux/amd64 nginx:alpine

# 1. DELETE DEFAULT NGINX PAGE (The "Welcome" file)
# This ensures that if something is wrong, you get a 403/404 instead of the Welcome page.
RUN rm -rf /usr/share/nginx/html/*

# 2. DELETE DEFAULT CONFIG
RUN rm /etc/nginx/conf.d/default.conf

# 3. Copy Custom Config
COPY nginx.conf /etc/nginx/conf.d/default.conf

# 4. Copy App
# Result: /usr/share/nginx/html/nature-help/index.html
COPY --from=frontend-build /usr/src/app/dist/ /usr/share/nginx/html/

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]