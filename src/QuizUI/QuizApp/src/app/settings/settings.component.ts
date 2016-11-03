import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { ConfigService } from '../config-service.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
  baseUrl: string = null;
  apimKey: string = '';
  authHeader: string = null;

  constructor(private config: ConfigService) { }

  ngOnInit() {
    this.config.baseUrl.subscribe(url => this.baseUrl = url);
    this.config.apimKey.subscribe(key => this.apimKey = key);
  }

  setBaseUrl() {
    this.config.setBaseUrl(this.baseUrl);
  }

  setApimKey() {
    this.config.setAPIMKey(this.apimKey);
  }

  setAuthHeader() {
    this.config.setAuthToken(this.authHeader);
  }

  saveSettings() {
    this.setBaseUrl();
    this.setApimKey();
    this.setAuthHeader();
  }
}
