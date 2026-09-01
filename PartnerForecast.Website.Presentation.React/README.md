## Development

### Run
```command line
npm run dev
```

Open [http://localhost:3000](http://localhost:3000) to view.

Note, pay attention to the output of the command. It might indicate port 3000 is in use and it assigned it annother port.

### Config

#### env
The .env file contains the following which should be customized to your application
```
NEXT_PUBLIC_APP_NAME=Dice
NEXT_PUBLIC_APP_DESCRIPTION=Dice - roll a dice!
NEXT_PUBLIC_APP_COPYRIGHT=2026 CBIZ. All rights reserved.
NEXT_PUBLIC_APP_VERSION=1.0.0
```

The .env.dev file is included and set to point api to local host, https, port 7058. The default https port the template api projects run on.
```
NEXT_PUBLIC_API_URL=https://localhost:7058
```
If your application's local api is not running there, point this to where it needs to be. 
Not needed in non-local as code deployed out with a web api in via our standard pipelines will host the static files in the api.
This simplifies hosting (all on same port) and avoids CORS.

### favicon.ico

\src\app\favicon.ico will be the icon that shows up in the tab. Change it to be what is desired.




