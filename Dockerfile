# Etapa única: Servir el juego con Nginx
FROM nginx:latest

# Copiar los archivos de la build generada por GitHub Actions
COPY build/WebGL /usr/share/nginx/html/

# Reemplazar la configuración por defecto de Nginx para servir WebGL correctamente
RUN rm /etc/nginx/conf.d/default.conf
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Exponer el puerto 80 para servir el juego
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]

