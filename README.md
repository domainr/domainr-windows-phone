domainr-windows-phone
=====================

In order to build this project, you must create an xml file in the Domainr7 folder called FlurryConfig.xml, the contents of which should look like this:

```
<?xml version="1.0" encoding="utf-8"?>
<Flurry>
  <ApiKey>FLURRYAPIKEYGOESHERE</ApiKey>
</Flurry>
```

The Flurry API Key can be attained by creating yourself an account on flurry.com, creating an app, then getting the API key. The xml file is already part
of the project so Visual Studio will pick it up automatically.

### Branches

- `master` contains the Windows Phone 7 code
- `WP8` contains the Windows Phone 8 code
