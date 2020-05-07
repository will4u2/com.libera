import { IBaseClass } from './i-base-class';

export interface ICoinType extends IBaseClass {
  type?: string;
  value?: number;
}
