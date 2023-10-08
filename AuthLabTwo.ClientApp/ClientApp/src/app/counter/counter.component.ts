import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { WeatherForecast } from '../fetch-data/fetch-data.component';

@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html'
})
export class CounterComponent {

  constructor(http: HttpClient, @Inject('API_BASE_URL') baseUrl: string) {
    document.cookie = "myPreciousCookie=preciousValue; SameSite=None; Secure";
    http.get<WeatherForecast[]>("https://localhost:5002/" + 'weatherforecast').subscribe(result => {
      console.log(result);
    }, error => console.error(error));
  }

  public currentCount = 0;

  public incrementCounter() {
    this.currentCount++;
  }
}
