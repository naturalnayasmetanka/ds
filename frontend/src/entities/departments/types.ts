export type DepartmentDTO = {
  id: string;
  name: string;
  path: string;
  createdAt: Date;
  updatedAt: Date;
};

export type GetDepartmentsRequest = {
  pageNumber: number;
  pageSize: number;
};
