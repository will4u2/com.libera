import { IBaseClass } from './i-base-class';
import { ICoinType } from './i-coin-type';

export interface ICoin extends IBaseClass  {
  coinTypeId?: number;
  quantity?: number;
  coinType: ICoinType;
}
