import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { ScrollArea } from "@/components/ui/scroll-area";
import { Textarea } from "@/components/ui/textarea";
import { Label } from "@/components/ui/label";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { locaitonsAPI } from "@/entities/locations/api";
import { toast } from "sonner";
import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { Controller } from "react-hook-form";

type CreateLocationDialogProps = {
  isDialogOpen: boolean;
  setDialogOpen: (isOpen: boolean) => void;
};

const formSchema = z.object({
  name: z.string().min(1, "Укажите название"),
  timezone: z.string().min(1, "Выберите часовой пояс"),
  adress: z.object({
    country: z.string().min(1, "Укажите страну"),
    region: z.string().min(1, "Укажите регион"),
    settlementName: z.string().min(1, "Укажите населённый пункт"),
    settlementType: z.string().min(1, "Укажите тип населённого пункта"),
    street: z.string().min(1, "Укажите улицу"),
    buildingNumber: z.string().min(1, "Укажите номер дома"),
    buildingBlock: z.string().nullable(),
    entrance: z.string().min(1, "Укажите подъезд"),
    floor: z.string().min(1, "Укажите этаж"),
    premiseNumber: z.string().min(1, "Укажите номер помещения"),
    premiseType: z.string().min(1, "Укажите тип помещения"),
    postCode: z.string().min(1, "Укажите почтовый индекс"),
    fullAddress: z.string().min(1, "Укажите полный адрес"),
    comment: z.string().nullable(),
  }),
});

const defaultValues: FormValues = {
  name: "",
  timezone: "",
  adress: {
    country: "",
    region: "",
    settlementName: "",
    settlementType: "",
    street: "",
    buildingNumber: "",
    buildingBlock: "",
    entrance: "",
    floor: "",
    premiseNumber: "",
    premiseType: "",
    postCode: "",
    fullAddress: "",
    comment: "",
  },
};

type FormValues = z.infer<typeof formSchema>;

export default function CreateLocationDialog(props: CreateLocationDialogProps) {
  const queryClient = useQueryClient();

  const {
    register,
    handleSubmit,
    control,
    reset,
    formState: { errors },
  } = useForm<FormValues>({
    defaultValues: defaultValues,
    resolver: zodResolver(formSchema),
  });

  const { mutate, isPending } = useMutation({
    mutationFn: (data: FormValues) => locaitonsAPI.createLocation(data),
    onSettled: () => queryClient.invalidateQueries({ queryKey: ["locations"] }),
    onError: () => toast.error("Ошибка создания"),
    onSuccess: () => {
      props.setDialogOpen(false);
      reset(defaultValues);
      toast.success("Локация успешно создана");
    },
  });

  const onSubmit = (data: FormValues) => {
    mutate(data);
  };

  const onClean = () => {
    reset(defaultValues);
    props.setDialogOpen(false);
  };

  return (
    <Dialog open={props.isDialogOpen} onOpenChange={props.setDialogOpen}>
      <DialogContent className="sm:max-w-[640px]">
        <DialogHeader>
          <DialogTitle>Новая локация</DialogTitle>
          <DialogDescription>
            Заполните данные о локации и адресе. Поля, отмеченные звёздочкой,
            обязательны.
          </DialogDescription>
        </DialogHeader>

        <form className="flex flex-col gap-4" onSubmit={handleSubmit(onSubmit)}>
          <ScrollArea className="max-h-[60vh] pr-4">
            <div className="flex flex-col gap-4">
              {/* Основные поля */}
              <div className="grid grid-cols-2 gap-4">
                <div className="flex flex-col gap-2">
                  <Label htmlFor="name">Название *</Label>
                  <Input
                    id="name"
                    placeholder="Склад №1"
                    {...register("name")}
                  />
                  {errors.name && (
                    <p className="text-sm text-destructive">
                      {errors.name.message}
                    </p>
                  )}
                </div>

                <div className="flex flex-col gap-2">
                  <Label htmlFor="timezone">Часовой пояс *</Label>
                  <Controller
                    name="timezone"
                    control={control}
                    render={({ field, fieldState }) => (
                      <>
                        <Select
                          onValueChange={field.onChange}
                          value={field.value}
                        >
                          <SelectTrigger id="timezone">
                            <SelectValue placeholder="Выберите часовой пояс" />
                          </SelectTrigger>
                          <SelectContent>
                            <SelectItem value="Europe/Moscow">
                              Europe/Moscow
                            </SelectItem>
                            <SelectItem value="Europe/Kaliningrad">
                              Europe/Kaliningrad
                            </SelectItem>
                            <SelectItem value="Europe/Warsaw">
                              Europe/Warsaw
                            </SelectItem>
                            <SelectItem value="Asia/Yekaterinburg">
                              Asia/Yekaterinburg
                            </SelectItem>
                            <SelectItem value="Asia/Novosibirsk">
                              Asia/Novosibirsk
                            </SelectItem>
                            <SelectItem value="Asia/Vladivostok">
                              Asia/Vladivostok
                            </SelectItem>
                          </SelectContent>
                        </Select>

                        {fieldState.error && (
                          <p className="text-sm text-destructive">
                            {fieldState.error.message}
                          </p>
                        )}
                      </>
                    )}
                  />
                </div>
              </div>

              <div className="h-px bg-border" />

              {/* Адрес */}
              <p className="text-sm font-medium text-muted-foreground">Адрес</p>

              <div className="grid grid-cols-2 gap-4">
                <div className="flex flex-col gap-2">
                  <Label htmlFor="country">Страна *</Label>
                  <Input
                    id="country"
                    placeholder="Россия"
                    {...register("adress.country")}
                  />
                  {errors.adress?.country && (
                    <p className="text-sm text-destructive">
                      {errors.adress?.country?.message}
                    </p>
                  )}
                </div>

                <div className="flex flex-col gap-2">
                  <Label htmlFor="region">Регион *</Label>
                  <Input
                    id="region"
                    placeholder="Московская область"
                    {...register("adress.region")}
                  />
                  {errors.adress?.region && (
                    <p className="text-sm text-destructive">
                      {errors.adress?.region?.message}
                    </p>
                  )}
                </div>
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div className="flex flex-col gap-2">
                  <Label htmlFor="settlementType">Тип н.п. *</Label>
                  <Controller
                    name="adress.settlementType"
                    control={control}
                    render={({ field, fieldState }) => (
                      <>
                        <Select
                          onValueChange={field.onChange}
                          value={field.value}
                        >
                          <SelectTrigger id="settlementType">
                            <SelectValue placeholder="Тип" />
                          </SelectTrigger>
                          <SelectContent>
                            <SelectItem value="город">город</SelectItem>
                            <SelectItem value="посёлок">посёлок</SelectItem>
                            <SelectItem value="село">село</SelectItem>
                            <SelectItem value="деревня">деревня</SelectItem>
                            <SelectItem value="хутор">хутор</SelectItem>
                          </SelectContent>
                        </Select>
                        {fieldState.error && (
                          <p className="text-sm text-destructive">
                            {fieldState.error.message}
                          </p>
                        )}
                      </>
                    )}
                  />
                </div>

                <div className="flex flex-col gap-2">
                  <Label htmlFor="settlementName">Населённый пункт *</Label>
                  <Input
                    id="settlementName"
                    placeholder="Пушкино"
                    {...register("adress.settlementName")}
                  />
                  {errors.adress?.settlementName && (
                    <p className="text-sm text-destructive">
                      {errors.adress?.settlementName?.message}
                    </p>
                  )}
                </div>
              </div>

              <div className="flex flex-col gap-2">
                <Label htmlFor="street">Улица *</Label>
                <Input
                  id="street"
                  placeholder="ул. Ленина"
                  {...register("adress.street")}
                />
                {errors.adress?.street && (
                  <p className="text-sm text-destructive">
                    {errors.adress?.street?.message}
                  </p>
                )}
              </div>

              <div className="grid grid-cols-3 gap-4">
                <div className="flex flex-col gap-2">
                  <Label htmlFor="buildingNumber">Дом *</Label>
                  <Input
                    id="buildingNumber"
                    placeholder="12"
                    {...register("adress.buildingNumber")}
                  />
                  {errors.adress?.buildingNumber && (
                    <p className="text-sm text-destructive">
                      {errors.adress?.buildingNumber?.message}
                    </p>
                  )}
                </div>

                <div className="flex flex-col gap-2">
                  <Label htmlFor="buildingBlock">Корпус</Label>
                  <Input
                    id="buildingBlock"
                    placeholder="А"
                    {...register("adress.buildingBlock")}
                  />
                  {errors.adress?.buildingBlock && (
                    <p className="text-sm text-destructive">
                      {errors.adress?.buildingBlock?.message}
                    </p>
                  )}
                </div>

                <div className="flex flex-col gap-2">
                  <Label htmlFor="entrance">Подъезд *</Label>
                  <Input
                    id="entrance"
                    placeholder="2"
                    {...register("adress.entrance")}
                  />
                  {errors.adress?.entrance && (
                    <p className="text-sm text-destructive">
                      {errors.adress?.entrance?.message}
                    </p>
                  )}
                </div>
              </div>

              <div className="grid grid-cols-3 gap-4">
                <div className="flex flex-col gap-2">
                  <Label htmlFor="floor">Этаж *</Label>
                  <Input
                    id="floor"
                    placeholder="3"
                    {...register("adress.floor")}
                  />
                  {errors.adress?.floor && (
                    <p className="text-sm text-destructive">
                      {errors.adress?.floor?.message}
                    </p>
                  )}
                </div>

                <div className="flex flex-col gap-2">
                  <Label htmlFor="premiseType">Тип помещения *</Label>

                  <Controller
                    name="adress.premiseType"
                    control={control}
                    render={({ field, fieldState }) => (
                      <>
                        <Select
                          onValueChange={field.onChange}
                          value={field.value}
                        >
                          <SelectTrigger id="premiseType">
                            <SelectValue placeholder="Тип" />
                          </SelectTrigger>
                          <SelectContent>
                            <SelectItem value="офис">офис</SelectItem>
                            <SelectItem value="квартира">квартира</SelectItem>
                            <SelectItem value="помещение">помещение</SelectItem>
                            <SelectItem value="склад">склад</SelectItem>
                            <SelectItem value="магазин">магазин</SelectItem>
                          </SelectContent>
                        </Select>
                        {fieldState.error && (
                          <p className="text-sm text-destructive">
                            {fieldState.error.message}
                          </p>
                        )}
                      </>
                    )}
                  />
                </div>

                <div className="flex flex-col gap-2">
                  <Label htmlFor="premiseNumber">Номер помещ. *</Label>
                  <Input
                    id="premiseNumber"
                    placeholder="305"
                    {...register("adress.premiseNumber")}
                  />
                  {errors.adress?.premiseNumber && (
                    <p className="text-sm text-destructive">
                      {errors.adress?.premiseNumber?.message}
                    </p>
                  )}
                </div>
              </div>

              <div className="flex flex-col gap-2">
                <Label htmlFor="postCode">Почтовый индекс *</Label>
                <Input
                  id="postCode"
                  placeholder="141207"
                  {...register("adress.postCode")}
                />
                {errors.adress?.postCode && (
                  <p className="text-sm text-destructive">
                    {errors.adress?.postCode?.message}
                  </p>
                )}
              </div>

              <div className="flex flex-col gap-2">
                <Label htmlFor="fullAddress">Полный адрес *</Label>
                <Input
                  id="fullAddress"
                  placeholder="Россия, Московская область, г. Пушкино, ул. Ленина, д. 12"
                  {...register("adress.fullAddress")}
                />
                {errors.adress?.fullAddress && (
                  <p className="text-sm text-destructive">
                    {errors.adress?.fullAddress?.message}
                  </p>
                )}
              </div>

              <div className="flex flex-col gap-2">
                <Label htmlFor="comment">Комментарий</Label>
                <Textarea
                  id="comment"
                  placeholder="Дополнительная информация по адресу"
                  {...register("adress.comment")}
                />
                {errors.adress?.comment && (
                  <p className="text-sm text-destructive">
                    {errors.adress?.comment?.message}
                  </p>
                )}
              </div>
            </div>
          </ScrollArea>

          <DialogFooter>
            <Button type="button" variant="outline" onClick={() => onClean()}>
              Отмена
            </Button>
            <Button type="submit" disabled={isPending}>
              {isPending ? "Сохранение..." : "Создать"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
