import { Component, OnInit, Inject } from '@angular/core';
import { ICoin } from '../_models/i-coin';
import { TillDataService } from '../core/till-data.service';

@Component({
  selector: 'app-change-maker',
  templateUrl: './change-maker.component.html',
  styleUrls: ['./change-maker.component.css']
})
export class ChangeMakerComponent implements OnInit {
  amountToChange: number;
  coins: ICoin[] = new Array();
  till: ICoin[] = new Array();
  initialtill: ICoin[] = new Array();
  constructor(@Inject(TillDataService) private tillDataService: TillDataService) { }

  ngOnInit(): void {
    this.tillDataService.getTill().subscribe(
      (data) => {
        this.till = (data as ICoin[]);
        this.initialtill = (data as ICoin[]);
      }
    );
  }

  getTill() {
    this.initialtill = this.till;
    this.tillDataService.getTill().subscribe(
      (data) => {
        this.till = (data as ICoin[]);
      }
    );
  }

  getChange() {
    this.tillDataService.getChange(this.amountToChange/100).subscribe(
      (data) => {
        this.coins = (data as ICoin[]);
        this.getTill();
      }
    );
  }
}
