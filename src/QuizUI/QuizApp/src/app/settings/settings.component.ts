import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { ConfigService } from '../config-service.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
  private _baseUrl: string = null;

  constructor(private config: ConfigService) { }

  ngOnInit() {
    this.config.baseUrl.subscribe(url => this._baseUrl = url);
  }

  get baseUrl(): string {
    return this._baseUrl;
  }

  set baseUrl(url: string) {
    this.config.setBaseUrl(url);
  }
}
