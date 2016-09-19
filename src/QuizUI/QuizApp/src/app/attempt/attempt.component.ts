import { Component, OnInit } from '@angular/core';
import { AttemptNavigatorService } from '../attempt-navigator.service';

@Component({
  selector: 'app-attempt',
  templateUrl: './attempt.component.html',
  styleUrls: ['./attempt.component.css']
})
export class AttemptComponent implements OnInit {

  constructor(private attemptNav: AttemptNavigatorService) { }

  ngOnInit() {
  }

}
