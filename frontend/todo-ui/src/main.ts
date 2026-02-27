import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { App } from './app/app';
import { importProvidersFrom } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

// ensure HttpClient is available to standalone components
const combinedConfig = {
  ...appConfig,
  providers: [
    ...(appConfig?.providers ?? []),
    importProvidersFrom(HttpClientModule)
  ]
};

bootstrapApplication(App, combinedConfig)
  .catch((err) => console.error(err));
