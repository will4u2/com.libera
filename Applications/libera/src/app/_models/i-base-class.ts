export interface IBaseClass {
  id?: number;
  active?: boolean;
  inUser?: number;
  inApplication?: number;
  inDate?: Date;
  modificationUser?: number;
  modificationApplication?: number;
  modificationDate?: Date;
  deleteUser?: number;
  deleteApplication?: number;
  deleteDate?: Date;
}
