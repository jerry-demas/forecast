FROM cbizappdev-c4erg2gbekgvfxhd.azurecr.io/library/cert:latest AS cert
 
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app
 
COPY . .
 
COPY --from=cert ./*.crt /usr/local/share/ca-certificates/

RUN apt-get update \
	&& apt-get install -y ca-certificates \
	&& update-ca-certificates \
	&& apt-get install krb5-user -y \
	&& ln -fs /usr/share/zoneinfo/America/New_York /etc/localtime \
	&& dpkg-reconfigure --frontend noninteractive tzdata \
    && apt-get update \
	&& apt-get clean \
	&& mkdir -p /app/logs \
	&& ln -fs /krb5/krb5cc /tmp/krb5cc_100000 #soft link to krb5 file

USER 100000 

# Expose the port
EXPOSE 5203
ENTRYPOINT ["dotnet", "/app/PartnerForecast.Website.Presentation.WebApi.dll"]
