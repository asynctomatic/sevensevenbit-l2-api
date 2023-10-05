FROM threesigmalabs.azurecr.io/threesigmaxyz/threesigma-starkex-contracts:latest

RUN apk update && apk add bash --no-cache

COPY entrypoint.sh entrypoint.sh
RUN chmod a+rx entrypoint.sh

ENTRYPOINT ["/bin/sh", "entrypoint.sh"] 