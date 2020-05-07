import { Component, OnInit, ViewChild, Inject } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ICoin } from '../_models/i-coin';
import { MatPaginator } from '@angular/material/paginator';
import { TillDataService } from '../core/till-data.service';

@Component({
  selector: 'app-till-manager',
  templateUrl: './till-manager.component.html',
  styleUrls: ['./till-manager.component.css']
})
export class TillManagerComponent implements OnInit {
  displayedColumns = ['id', 'type', 'quantity', 'value', 'total'];
  dataSource = new MatTableDataSource();
  coins: ICoin[];
  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  TillTotal: number;

  constructor(@Inject(TillDataService) private tillService: TillDataService) { }

  ngOnInit(): void {
    this.dataSource.paginator = this.paginator;
    this.tillService.getTill().subscribe(
      (data) => {
        console.log(data);
        this.coins = (data as ICoin[]);
        this.dataSource.data = (data as ICoin[]);
        this.TillTotal = this.coins.reduce((total, coin) => total + (coin.quantity * coin.type.value), 0);
      }
    );
  }

}
