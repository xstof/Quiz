import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { ConfigService } from '../config-service.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
  private baseUrl: string = null;

  constructor(private config: ConfigService) { }

  ngOnInit() {
    this.config.baseUrl.subscribe(url => this.baseUrl = url);
  }

  setBaseUrl() {
    this.config.setBaseUrl(this.baseUrl);
  }
}
