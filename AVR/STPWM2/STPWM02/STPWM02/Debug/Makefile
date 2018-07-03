################################################################################
# Automatically-generated file. Do not edit!
################################################################################

SHELL := cmd.exe
RM := rm -rf

USER_OBJS :=

LIBS := 
PROJ := 

O_SRCS := 
C_SRCS := 
S_SRCS := 
S_UPPER_SRCS := 
OBJ_SRCS := 
ASM_SRCS := 
PREPROCESSING_SRCS := 
OBJS := 
OBJS_AS_ARGS := 
C_DEPS := 
C_DEPS_AS_ARGS := 
EXECUTABLES := 
OUTPUT_FILE_PATH :=
OUTPUT_FILE_PATH_AS_ARGS :=
AVR_APP_PATH :=$$$AVR_APP_PATH$$$
QUOTE := "
ADDITIONAL_DEPENDENCIES:=
OUTPUT_FILE_DEP:=
LIB_DEP:=
LINKER_SCRIPT_DEP:=

# Every subdirectory with source files must be described here
SUBDIRS :=  \
../src/ \
../src/ASF/ \
../src/ASF/common/ \
../src/ASF/common/boards/ \
../src/ASF/common/boards/user_board/ \
../src/ASF/common/services/ \
../src/ASF/common/services/calendar/ \
../src/ASF/common/services/clock/ \
../src/ASF/common/services/clock/mega/ \
../src/ASF/common/services/delay/ \
../src/ASF/common/services/delay/mega/ \
../src/ASF/common/services/gpio/ \
../src/ASF/common/services/gpio/mega_gpio/ \
../src/ASF/common/services/ioport/ \
../src/ASF/common/services/ioport/mega/ \
../src/ASF/common/utils/ \
../src/ASF/common/utils/interrupt/ \
../src/ASF/common/utils/make/ \
../src/ASF/common/utils/stdio/ \
../src/ASF/common/utils/unit_test/ \
../src/ASF/mega/ \
../src/ASF/mega/drivers/ \
../src/ASF/mega/drivers/adc/ \
../src/ASF/mega/drivers/cpu/ \
../src/ASF/mega/utils/ \
../src/ASF/mega/utils/assembler/ \
../src/ASF/mega/utils/preprocessor/ \
../src/config/


# Add inputs and outputs from these tool invocations to the build variables 
C_SRCS +=  \
../src/eeprom_store.c \
../src/terminal.c \
../src/ASF/common/boards/user_board/init.c \
../src/ASF/common/services/calendar/calendar.c \
../src/ASF/common/services/clock/mega/sysclk.c \
../src/ASF/common/utils/stdio/read.c \
../src/ASF/common/utils/stdio/write.c \
../src/ASF/common/utils/unit_test/suite.c \
../src/crc16.c \
../src/main.c


PREPROCESSING_SRCS += 


ASM_SRCS += 


OBJS +=  \
src/eeprom_store.o \
src/terminal.o \
src/ASF/common/boards/user_board/init.o \
src/ASF/common/services/calendar/calendar.o \
src/ASF/common/services/clock/mega/sysclk.o \
src/ASF/common/utils/stdio/read.o \
src/ASF/common/utils/stdio/write.o \
src/ASF/common/utils/unit_test/suite.o \
src/crc16.o \
src/main.o

OBJS_AS_ARGS +=  \
src/eeprom_store.o \
src/terminal.o \
src/ASF/common/boards/user_board/init.o \
src/ASF/common/services/calendar/calendar.o \
src/ASF/common/services/clock/mega/sysclk.o \
src/ASF/common/utils/stdio/read.o \
src/ASF/common/utils/stdio/write.o \
src/ASF/common/utils/unit_test/suite.o \
src/crc16.o \
src/main.o

C_DEPS +=  \
src/eeprom_store.d \
src/terminal.d \
src/ASF/common/boards/user_board/init.d \
src/ASF/common/services/calendar/calendar.d \
src/ASF/common/services/clock/mega/sysclk.d \
src/ASF/common/utils/stdio/read.d \
src/ASF/common/utils/stdio/write.d \
src/ASF/common/utils/unit_test/suite.d \
src/crc16.d \
src/main.d

C_DEPS_AS_ARGS +=  \
src/eeprom_store.d \
src/terminal.d \
src/ASF/common/boards/user_board/init.d \
src/ASF/common/services/calendar/calendar.d \
src/ASF/common/services/clock/mega/sysclk.d \
src/ASF/common/utils/stdio/read.d \
src/ASF/common/utils/stdio/write.d \
src/ASF/common/utils/unit_test/suite.d \
src/crc16.d \
src/main.d

OUTPUT_FILE_PATH +=STPWM02.elf

OUTPUT_FILE_PATH_AS_ARGS +=STPWM02.elf

ADDITIONAL_DEPENDENCIES:=

OUTPUT_FILE_DEP:= ./makedep.mk

LIB_DEP+= 

LINKER_SCRIPT_DEP+= 


# AVR32/GNU C Compiler





















src/%.o: ../src/%.c
	@echo Building file: $<
	@echo Invoking: AVR/GNU C Compiler : 4.9.2
	$(QUOTE)C:\Program Files (x86)\Atmel\Studio\7.0\toolchain\avr8\avr8-gnu-toolchain\bin\avr-gcc.exe$(QUOTE)  -x c -DDEBUG -DBOARD=USER_BOARD -DTEST_SUITE_DEFINE_ASSERT_MACRO -D_ASSERT_ENABLE_  -I"C:\Program Files (x86)\Atmel\Studio\7.0\Packs\atmel\ATmega_DFP\1.0.90\include" -I"../common/applications/user_application/user_board/config" -I"../src/config" -I"../src/ASF/common/boards" -I"../src/ASF/mega/utils" -I"../src/ASF/common/utils" -I"../src/ASF/mega/utils/preprocessor" -I"../src" -I"../src/ASF/common/boards/user_board" -I"../src/ASF/common/services/calendar" -I"../src/ASF/mega/drivers/cpu" -I"../src/ASF/common/services/clock" -I"../src/ASF/common/services/delay" -I"../src/ASF/common/services/gpio" -I"../src/ASF/common/services/ioport" -I"../src/ASF/mega/drivers/adc" -I".."  -O1 -fdata-sections -ffunction-sections -fdata-sections -fpack-struct -fshort-enums -g3 -Wall -mmcu=atmega164pa -B "C:\Program Files (x86)\Atmel\Studio\7.0\Packs\atmel\ATmega_DFP\1.0.90\gcc\dev\atmega164pa" -c -std=gnu99 -fno-strict-aliasing -Wstrict-prototypes -Wmissing-prototypes -Werror-implicit-function-declaration -Wpointer-arith -mrelax -MD -MP -MF "$(@:%.o=%.d)" -MT"$(@:%.o=%.d)" -MT"$(@:%.o=%.o)"   -o "$@" "$<" 
	@echo Finished building: $<
	

src/ASF/common/boards/user_board/%.o: ../src/ASF/common/boards/user_board/%.c
	@echo Building file: $<
	@echo Invoking: AVR/GNU C Compiler : 4.9.2
	$(QUOTE)C:\Program Files (x86)\Atmel\Studio\7.0\toolchain\avr8\avr8-gnu-toolchain\bin\avr-gcc.exe$(QUOTE)  -x c -DDEBUG -DBOARD=USER_BOARD -DTEST_SUITE_DEFINE_ASSERT_MACRO -D_ASSERT_ENABLE_  -I"C:\Program Files (x86)\Atmel\Studio\7.0\Packs\atmel\ATmega_DFP\1.0.90\include" -I"../common/applications/user_application/user_board/config" -I"../src/config" -I"../src/ASF/common/boards" -I"../src/ASF/mega/utils" -I"../src/ASF/common/utils" -I"../src/ASF/mega/utils/preprocessor" -I"../src" -I"../src/ASF/common/boards/user_board" -I"../src/ASF/common/services/calendar" -I"../src/ASF/mega/drivers/cpu" -I"../src/ASF/common/services/clock" -I"../src/ASF/common/services/delay" -I"../src/ASF/common/services/gpio" -I"../src/ASF/common/services/ioport" -I"../src/ASF/mega/drivers/adc" -I".."  -O1 -fdata-sections -ffunction-sections -fdata-sections -fpack-struct -fshort-enums -g3 -Wall -mmcu=atmega164pa -B "C:\Program Files (x86)\Atmel\Studio\7.0\Packs\atmel\ATmega_DFP\1.0.90\gcc\dev\atmega164pa" -c -std=gnu99 -fno-strict-aliasing -Wstrict-prototypes -Wmissing-prototypes -Werror-implicit-function-declaration -Wpointer-arith -mrelax -MD -MP -MF "$(@:%.o=%.d)" -MT"$(@:%.o=%.d)" -MT"$(@:%.o=%.o)"   -o "$@" "$<" 
	@echo Finished building: $<
	

src/ASF/common/services/calendar/%.o: ../src/ASF/common/services/calendar/%.c
	@echo Building file: $<
	@echo Invoking: AVR/GNU C Compiler : 4.9.2
	$(QUOTE)C:\Program Files (x86)\Atmel\Studio\7.0\toolchain\avr8\avr8-gnu-toolchain\bin\avr-gcc.exe$(QUOTE)  -x c -DDEBUG -DBOARD=USER_BOARD -DTEST_SUITE_DEFINE_ASSERT_MACRO -D_ASSERT_ENABLE_  -I"C:\Program Files (x86)\Atmel\Studio\7.0\Packs\atmel\ATmega_DFP\1.0.90\include" -I"../common/applications/user_application/user_board/config" -I"../src/config" -I"../src/ASF/common/boards" -I"../src/ASF/mega/utils" -I"../src/ASF/common/utils" -I"../src/ASF/mega/utils/preprocessor" -I"../src" -I"../src/ASF/common/boards/user_board" -I"../src/ASF/common/services/calendar" -I"../src/ASF/mega/drivers/cpu" -I"../src/ASF/common/services/clock" -I"../src/ASF/common/services/delay" -I"../src/ASF/common/services/gpio" -I"../src/ASF/common/services/ioport" -I"../src/ASF/mega/drivers/adc" -I".."  -O1 -fdata-sections -ffunction-sections -fdata-sections -fpack-struct -fshort-enums -g3 -Wall -mmcu=atmega164pa -B "C:\Program Files (x86)\Atmel\Studio\7.0\Packs\atmel\ATmega_DFP\1.0.90\gcc\dev\atmega164pa" -c -std=gnu99 -fno-strict-aliasing -Wstrict-prototypes -Wmissing-prototypes -Werror-implicit-function-declaration -Wpointer-arith -mrelax -MD -MP -MF "$(@:%.o=%.d)" -MT"$(@:%.o=%.d)" -MT"$(@:%.o=%.o)"   -o "$@" "$<" 
	@echo Finished building: $<
	

src/ASF/common/services/clock/mega/%.o: ../src/ASF/common/services/clock/mega/%.c
	@echo Building file: $<
	@echo Invoking: AVR/GNU C Compiler : 4.9.2
	$(QUOTE)C:\Program Files (x86)\Atmel\Studio\7.0\toolchain\avr8\avr8-gnu-toolchain\bin\avr-gcc.exe$(QUOTE)  -x c -DDEBUG -DBOARD=USER_BOARD -DTEST_SUITE_DEFINE_ASSERT_MACRO -D_ASSERT_ENABLE_  -I"C:\Program Files (x86)\Atmel\Studio\7.0\Packs\atmel\ATmega_DFP\1.0.90\include" -I"../common/applications/user_application/user_board/config" -I"../src/config" -I"../src/ASF/common/boards" -I"../src/ASF/mega/utils" -I"../src/ASF/common/utils" -I"../src/ASF/mega/utils/preprocessor" -I"../src" -I"../src/ASF/common/boards/user_board" -I"../src/ASF/common/services/calendar" -I"../src/ASF/mega/drivers/cpu" -I"../src/ASF/common/services/clock" -I"../src/ASF/common/services/delay" -I"../src/ASF/common/services/gpio" -I"../src/ASF/common/services/ioport" -I"../src/ASF/mega/drivers/adc" -I".."  -O1 -fdata-sections -ffunction-sections -fdata-sections -fpack-struct -fshort-enums -g3 -Wall -mmcu=atmega164pa -B "C:\Program Files (x86)\Atmel\Studio\7.0\Packs\atmel\ATmega_DFP\1.0.90\gcc\dev\atmega164pa" -c -std=gnu99 -fno-strict-aliasing -Wstrict-prototypes -Wmissing-prototypes -Werror-implicit-function-declaration -Wpointer-arith -mrelax -MD -MP -MF "$(@:%.o=%.d)" -MT"$(@:%.o=%.d)" -MT"$(@:%.o=%.o)"   -o "$@" "$<" 
	@echo Finished building: $<
	

src/ASF/common/utils/stdio/%.o: ../src/ASF/common/utils/stdio/%.c
	@echo Building file: $<
	@echo Invoking: AVR/GNU C Compiler : 4.9.2
	$(QUOTE)C:\Program Files (x86)\Atmel\Studio\7.0\toolchain\avr8\avr8-gnu-toolchain\bin\avr-gcc.exe$(QUOTE)  -x c -DDEBUG -DBOARD=USER_BOARD -DTEST_SUITE_DEFINE_ASSERT_MACRO -D_ASSERT_ENABLE_  -I"C:\Program Files (x86)\Atmel\Studio\7.0\Packs\atmel\ATmega_DFP\1.0.90\include" -I"../common/applications/user_application/user_board/config" -I"../src/config" -I"../src/ASF/common/boards" -I"../src/ASF/mega/utils" -I"../src/ASF/common/utils" -I"../src/ASF/mega/utils/preprocessor" -I"../src" -I"../src/ASF/common/boards/user_board" -I"../src/ASF/common/services/calendar" -I"../src/ASF/mega/drivers/cpu" -I"../src/ASF/common/services/clock" -I"../src/ASF/common/services/delay" -I"../src/ASF/common/services/gpio" -I"../src/ASF/common/services/ioport" -I"../src/ASF/mega/drivers/adc" -I".."  -O1 -fdata-sections -ffunction-sections -fdata-sections -fpack-struct -fshort-enums -g3 -Wall -mmcu=atmega164pa -B "C:\Program Files (x86)\Atmel\Studio\7.0\Packs\atmel\ATmega_DFP\1.0.90\gcc\dev\atmega164pa" -c -std=gnu99 -fno-strict-aliasing -Wstrict-prototypes -Wmissing-prototypes -Werror-implicit-function-declaration -Wpointer-arith -mrelax -MD -MP -MF "$(@:%.o=%.d)" -MT"$(@:%.o=%.d)" -MT"$(@:%.o=%.o)"   -o "$@" "$<" 
	@echo Finished building: $<
	

src/ASF/common/utils/unit_test/%.o: ../src/ASF/common/utils/unit_test/%.c
	@echo Building file: $<
	@echo Invoking: AVR/GNU C Compiler : 4.9.2
	$(QUOTE)C:\Program Files (x86)\Atmel\Studio\7.0\toolchain\avr8\avr8-gnu-toolchain\bin\avr-gcc.exe$(QUOTE)  -x c -DDEBUG -DBOARD=USER_BOARD -DTEST_SUITE_DEFINE_ASSERT_MACRO -D_ASSERT_ENABLE_  -I"C:\Program Files (x86)\Atmel\Studio\7.0\Packs\atmel\ATmega_DFP\1.0.90\include" -I"../common/applications/user_application/user_board/config" -I"../src/config" -I"../src/ASF/common/boards" -I"../src/ASF/mega/utils" -I"../src/ASF/common/utils" -I"../src/ASF/mega/utils/preprocessor" -I"../src" -I"../src/ASF/common/boards/user_board" -I"../src/ASF/common/services/calendar" -I"../src/ASF/mega/drivers/cpu" -I"../src/ASF/common/services/clock" -I"../src/ASF/common/services/delay" -I"../src/ASF/common/services/gpio" -I"../src/ASF/common/services/ioport" -I"../src/ASF/mega/drivers/adc" -I".."  -O1 -fdata-sections -ffunction-sections -fdata-sections -fpack-struct -fshort-enums -g3 -Wall -mmcu=atmega164pa -B "C:\Program Files (x86)\Atmel\Studio\7.0\Packs\atmel\ATmega_DFP\1.0.90\gcc\dev\atmega164pa" -c -std=gnu99 -fno-strict-aliasing -Wstrict-prototypes -Wmissing-prototypes -Werror-implicit-function-declaration -Wpointer-arith -mrelax -MD -MP -MF "$(@:%.o=%.d)" -MT"$(@:%.o=%.d)" -MT"$(@:%.o=%.o)"   -o "$@" "$<" 
	@echo Finished building: $<
	



# AVR32/GNU Preprocessing Assembler



# AVR32/GNU Assembler




ifneq ($(MAKECMDGOALS),clean)
ifneq ($(strip $(C_DEPS)),)
-include $(C_DEPS)
endif
endif

# Add inputs and outputs from these tool invocations to the build variables 

# All Target
all: $(OUTPUT_FILE_PATH) $(ADDITIONAL_DEPENDENCIES)

$(OUTPUT_FILE_PATH): $(OBJS) $(USER_OBJS) $(OUTPUT_FILE_DEP) $(LIB_DEP) $(LINKER_SCRIPT_DEP)
	@echo Building target: $@
	@echo Invoking: AVR/GNU Linker : 4.9.2
	$(QUOTE)C:\Program Files (x86)\Atmel\Studio\7.0\toolchain\avr8\avr8-gnu-toolchain\bin\avr-gcc.exe$(QUOTE) -o$(OUTPUT_FILE_PATH_AS_ARGS) $(OBJS_AS_ARGS) $(USER_OBJS) $(LIBS) -Wl,-Map="STPWM02.map" -Wl,--start-group -Wl,-lprintf_flt -Wl,-lm  -Wl,--end-group -Wl,--gc-sections -mmcu=atmega164pa -B "C:\Program Files (x86)\Atmel\Studio\7.0\Packs\atmel\ATmega_DFP\1.0.90\gcc\dev\atmega164pa" -Wl,--relax  
	@echo Finished building target: $@
	"C:\Program Files (x86)\Atmel\Studio\7.0\toolchain\avr8\avr8-gnu-toolchain\bin\avr-objcopy.exe" -O ihex -R .eeprom -R .fuse -R .lock -R .signature -R .user_signatures  "STPWM02.elf" "STPWM02.hex"
	"C:\Program Files (x86)\Atmel\Studio\7.0\toolchain\avr8\avr8-gnu-toolchain\bin\avr-objcopy.exe" -j .eeprom  --set-section-flags=.eeprom=alloc,load --change-section-lma .eeprom=0  --no-change-warnings -O ihex "STPWM02.elf" "STPWM02.eep" || exit 0
	"C:\Program Files (x86)\Atmel\Studio\7.0\toolchain\avr8\avr8-gnu-toolchain\bin\avr-objdump.exe" -h -S "STPWM02.elf" > "STPWM02.lss"
	"C:\Program Files (x86)\Atmel\Studio\7.0\toolchain\avr8\avr8-gnu-toolchain\bin\avr-objcopy.exe" -O srec -R .eeprom -R .fuse -R .lock -R .signature -R .user_signatures "STPWM02.elf" "STPWM02.srec"
	"C:\Program Files (x86)\Atmel\Studio\7.0\toolchain\avr8\avr8-gnu-toolchain\bin\avr-size.exe" "STPWM02.elf"
	
	





# Other Targets
clean:
	-$(RM) $(OBJS_AS_ARGS) $(EXECUTABLES)  
	-$(RM) $(C_DEPS_AS_ARGS)   
	rm -rf "STPWM02.elf" "STPWM02.a" "STPWM02.hex" "STPWM02.lss" "STPWM02.eep" "STPWM02.map" "STPWM02.srec" "STPWM02.usersignatures"
	