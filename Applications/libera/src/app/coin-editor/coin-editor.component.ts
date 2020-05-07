import { Component, OnInit, Inject } from '@angular/core';
import { ICoin } from '../_models/i-coin';
import { TillDataService } from '../core/till-data.service';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-coin-editor',
  templateUrl: './coin-editor.component.html',
  styleUrls: ['./coin-editor.component.css']
})
export class CoinEditorComponent implements OnInit {
  production: boolean = environment.production;
  coin: ICoin;
  coins: ICoin[] = new Array();
  constructor(private route: ActivatedRoute, private router: Router, @Inject(TillDataService) private tillDataService: TillDataService) { }

  ngOnInit(): void {
    const coinId = this.route.snapshot.params.id;
    this.tillDataService.getTill().subscribe(
      (data) => {
        this.coin = (data as ICoin[]).find(c => c.id === parseInt(coinId));
      }
    );
  }

  saveCoins() {
    this.coins.push(this.coin);
    this.tillDataService.patchTill(this.coins).subscribe(
      (data) => {
        this.router.navigate(['']);
      }
    );
  }
}
