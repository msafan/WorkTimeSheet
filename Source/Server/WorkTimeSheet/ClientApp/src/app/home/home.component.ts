import { Component, OnInit } from '@angular/core';

declare function initializeJS(): void;

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})

export class HomeComponent implements OnInit {
  
  ngOnInit(): void {
    initializeJS();
  }
}
