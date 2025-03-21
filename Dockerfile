# Usar una imagen base de Nginx con soporte para Brotli
FROM nginx:latest

# Instalar el módulo Brotli
RUN apt-get update && apt-get install -y nginx-module-brotli

# Habilitar el módulo Brotli
RUN echo "load_module modules/ngx_http_brotli_filter_module.so;" > /etc/nginx/modules-enabled/50-module-brotli.conf
RUN echo "load_module modules/ngx_http_brotli_static_module.so;" >> /etc/nginx/modules-enabled/50-module-brotli.conf

# Copiar los archivos de la build generada por GitHub Actions
COPY build/WebGL/ /usr/share/nginx/html/

# Reemplazar la configuración por defecto de Nginx para servir WebGL correctamente
RUN rm /etc/nginx/conf.d/default.conf
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Exponer el puerto 80 para servir el juego
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
