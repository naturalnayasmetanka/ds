import { apiClient } from "@/shared/api/axios.instance";
import { PaginationResponse } from "@/shared/api/types";
import { DepartmentDTO, GetDepartmentsRequest } from "./types";

export const departmentsAPI = {
  getDepartments: async (
    request: GetDepartmentsRequest,
  ): Promise<PaginationResponse<DepartmentDTO>> => {
    const locations = await apiClient.get<PaginationResponse<DepartmentDTO>>(
      "/departments",
      {
        params: request,
      },
    );

    return locations.data;
  },
};
