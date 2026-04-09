# Making GHCR Images Public

After the first CD pipeline run, make the packages public so they are visible
on the GitHub profile (required for portfolio visibility):

1. Go to https://github.com/okalangkenneth?tab=packages
2. Click each `skillfind-*` package
3. Click **Package settings** (bottom right)
4. Scroll to **Danger Zone** → Change visibility → Public
5. Confirm

Repeat for all 6 packages. Once public, the images appear on the GitHub profile
and anyone can pull them with:

```
docker pull ghcr.io/okalangkenneth/skillfind-jobposting-api:latest
docker pull ghcr.io/okalangkenneth/skillfind-jobcategory-api:latest
docker pull ghcr.io/okalangkenneth/skillfind-jobseeker-api:latest
docker pull ghcr.io/okalangkenneth/skillfind-notification-service:latest
docker pull ghcr.io/okalangkenneth/skillfind-search-service:latest
docker pull ghcr.io/okalangkenneth/skillfind-apigateway:latest
```
